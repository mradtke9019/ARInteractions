import glob
import json
import os
from pathlib import Path
import numpy as np
import pandas as pd
from sklearn.utils import shuffle
from sklearn import preprocessing
from sklearn.preprocessing import PolynomialFeatures
from sklearn.model_selection import KFold
from sklearn.neighbors import KNeighborsClassifier
from sklearn.neighbors import KNeighborsRegressor 
from sklearn.metrics import classification_report
from sklearn.metrics import confusion_matrix
from sklearn.dummy import DummyClassifier
from sklearn.model_selection import cross_val_score
from sklearn.metrics import roc_curve
from sklearn.model_selection import train_test_split
import matplotlib.patches as mpatches
from sklearn import linear_model
from sklearn.metrics import mean_squared_error
import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D
from matplotlib import cm


# Merge all the json files into a single one with labels
def MergeJsonAndLabel(directory, destinationDir):
    outputKeys = {}
    outputKeys["None"] = 0
    outputKeys["Fist"] = 1
    outputKeys["Pinch"] = 2
    outputKeys["ThumbsUp"] = 3
    outputKeys["ThumbsDown"]=4
    outputKeys["Point"]=5
    outputKeys["Shaka"]=6
    outputKeys["SnapStart"]=7
    outputKeys["Palm"]=8
    outputKeys["WebSlinging"]=9
    outputKeys["PeaceSign"]=10
    jsonList = []
    # All files and directories ending with .txt and that don't begin with a dot:
    jsonFiles = glob.iglob(directory + '**/*.json', recursive=True)
    for file in jsonFiles:
        f = open(file)
        # returns JSON object as 
        # a dictionary
        path = Path(file)
        label = os.path.basename(os.path.normpath(path.parent.absolute()))
        # print(label)
        data = json.load(f)
        for jsonObj in data:
            jsonObj["Label"] = outputKeys[label]
            jsonList.append(jsonObj)
        # Closing file
        f.close()
    
    jsonStr = json.dumps([ob for ob in jsonList])
    outPath = os.path.join(destinationDir, "Final.json")
    with open(outPath, "w") as jsonFile:
        jsonFile.write(jsonStr)
    return outPath
    


class Dataset:
    def __init__(self, file,splitPercentage = .9, Debug = False):
        df = pd.read_json(file)
        if Debug:
            print(df.head())
            print(df.info())
        
        numRowsTrain = int(splitPercentage * df.shape[0])
        numRowsValidate = df.shape[0] - numRowsTrain
        #https://stackoverflow.com/questions/29576430/shuffle-dataframe-rows
        df = shuffle(df, random_state = 0)
        df.reset_index(inplace=True, drop=True)
        self.X = df.iloc[:,1:]
        self.y = df.iloc[:,0]
        self.ColumnNames = df.columns[1:]
        # https://scikit-learn.org/stable/modules/preprocessing.html
        scaler = preprocessing.StandardScaler().fit(self.X)
        self.XScaled = scaler.transform(self.X)
        
        self.trainX = self.X.iloc[:numRowsTrain]
        self.validateX = self.X.iloc[numRowsTrain:]
        
        self.trainXScaled = scaler.transform(self.trainX)
        self.validateXScaled = scaler.transform(self.validateX)
        
        self.trainY = self.y.iloc[:numRowsTrain]
        self.validateY = self.y.iloc[numRowsTrain:]
        
        self.xPolys = {}
        self.xPolysScaled = {}
        self.trainxPolys= {}
        self.trainxPolysScaled = {}
        self.validatexPolys = {}
        self.validatexPolysScaled = {}
        self.polynomialFeatureNames = {}
    def AddPolynomialFeatures(self, degree):
        pf = PolynomialFeatures(degree)
        self.xPolys[degree] = pf.fit_transform(self.X)
        self.xPolysScaled[degree] = pf.fit_transform(self.XScaled)
        self.trainxPolys[degree] = pf.fit_transform(self.trainX)
        self.trainxPolysScaled[degree] = pf.fit_transform(self.trainXScaled)
        self.validatexPolys[degree] = pf.fit_transform(self.validateX)
        self.validatexPolysScaled[degree] = pf.fit_transform(self.validateXScaled)
        
        self.polynomialFeatureNames[degree] = pf.get_feature_names_out(self.ColumnNames)
    def PrintColumns(self):
        for name in self.ColumnNames:
            print(name)

class MLModel:
    def __init__(self):
        self.thetas = []
        self.type = None
        self.yPred = None
        self.model = None
    def TrainModel(self, ModelType, x, y, c = None, K = None):
        assert(self.type == None and ModelType in ["Lasso", "Ridge", "KNN"])
        self.type = ModelType
        if ModelType == "Lasso":
            self.model = linear_model.Lasso(alpha=(1/(2 * c)))
        elif ModelType == "Ridge":
            self.model = linear_model.Ridge(alpha=(1/(2 * c)))
        elif ModelType == "KNN":
            self.model = KNeighborsRegressor(n_neighbors = K)
            #assert (False)
        print("Fitting " + self.type)
        self.model.fit(x, y)
        
        if ModelType in ["Lasso", "Ridge"]:
            self.thetas.append(self.model.intercept_)
            for data in self.model.coef_:
                self.thetas.append(data)

    def KFoldsValidation(self, ModelType, x, y, hyperparameter = None, folds = 5):
        kf = KFold(n_splits = folds)
        assert(self.type == None and ModelType in ["Lasso", "Ridge", "KNN"])
        self.type = ModelType
        self.meanError = []
        self.stdError = []
        # Use current polynomial features and 
        # C value to perform k folds validation
        if ModelType == "Lasso":
            self.model = linear_model.Lasso(alpha=(1/(2 * hyperparameter)))
        elif ModelType == "Ridge":
            self.model = linear_model.Ridge(alpha=(1/(2 * hyperparameter)))
        elif ModelType == "KNN":
            return self.KFoldsKNN(x,y,hyperparameter, folds)
            
        
        temp = []
        for train,test in kf.split(x):
            self.model.fit(x[train], y[train])
            yPred = self.model.predict(x[test])
            # append the F1 Score for the currently trained model
            temp.append(mean_squared_error(y[test],yPred))
        
        self.meanError = np.array(temp).mean()
        self.stdError = np.array(temp).std()
        return self.meanError, self.stdError
    
    def KFoldsKNN(self, x, y, K, folds = 5):
        kf = KFold(n_splits = folds)
        self.model = KNeighborsRegressor(n_neighbors = K)
        self.meanError = []
        self.stdError = []

        temp = []
        for train,test in kf.split(x):
            self.model.fit(x[train], y[train])
            yPred = self.model.predict(x[test])
            
            temp.append(mean_squared_error(y[test],yPred))
        
        self.meanError = np.array(temp).mean()
        self.stdError = np.array(temp).std()
        return self.meanError, self.stdError
    def Predict(self, x, y):
        assert self.type != None
        yPred = self.model.predict(x)
        mse = mean_squared_error(y,yPred)
        return mse
    
    def PrintWeights(self, names):
        assert(self.type in ["Lasso", "Ridge"])
        weights = self.thetas[1:]
        print(len(names), len(weights))
        for i in range(len(weights)):
            print(names[i], weights[i])
            
    # https://onnx.ai/sklearn-onnx/
    def ExportModelToONNX(self, path = "./"):
        from skl2onnx import convert_sklearn
        from skl2onnx.common.data_types import FloatTensorType
        onx = convert_sklearn(self.model)
        targetPath = os.path.join(path, "Model" + self.ModelType +".onnx")
        with open(targetPath, "wb") as f:
            f.write(onx.SerializeToString())