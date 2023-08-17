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
from joblib import dump, load
from sklearn import svm
from PlotUtility import *
from sklearn.preprocessing import StandardScaler
from sklearn.svm import SVC
from sklearn.pipeline import make_pipeline
from sklearn.svm import LinearSVC

# Merge all the json files into a single one with labels
def MergeJsonAndLabel(directory, destinationDir, fileName = "Final.json", label="Classifier"):
    outputKeys = {}
    if label == "Classifier":
        outputKeys["None"] = "None"
        outputKeys["Fist"] = "Fist"
        outputKeys["Pinch"] = "Pinch"
        outputKeys["ThumbsUp"] = "ThumbsUp"
        outputKeys["ThumbsDown"]="ThumbsDown"
        outputKeys["Point"]="Point"
        outputKeys["Shaka"]="Shaka"
        outputKeys["SnapStart"]="SnapStart"
        outputKeys["Palm"]="Palm"
        outputKeys["WebSlinging"]="WebSlinging"
        outputKeys["PeaceSign"]="PeaceSign"
    else:
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
    outPath = os.path.join(destinationDir, fileName)
    with open(outPath, "w") as jsonFile:
        jsonFile.write(jsonStr)
    return outPath


# Given a dataset and some hyperparameters, plot the error. Returns the mean squared error and std deviation
def PerformKFoldsValidationAndPlot(modeltype, dataset, hyperparameters, hyperparameterLabel, yLabel = "Mean Squared Error",scale = False, Qs = None, folds = 5):
    kf = KFold(n_splits = folds)
    m = []
    std = []
    x = np.array(dataset.trainX)
    y = np.array(dataset.trainY)
    if scale:
        x = np.array(dataset.trainXScaled)

    # print("Length x and y", len(x), len(y))
    for h in hyperparameters:
        print(h)
        if modeltype == "KNN":
            model = KNeighborsClassifier(n_neighbors = h)
        elif modeltype == "SVC":
            model = SVC(C = 1/(2 * h))
        elif modeltype == "LinearSVCL1":
            model = LinearSVC(penalty='l1',loss='squared_hinge',C = 1/(2 * h), dual="auto")
        elif modeltype == "LinearSVCL2":
            model = LinearSVC(penalty='l2', C = 1/(2 * h),max_iter=1000)
        
        temp = []
        for train,test in kf.split(x):
            model.fit(x[train], y[train])
            yPred = model.predict(x[test])
            # Compare the prediction to the actual
            error = np.mean( yPred != y[test] )
            score = 1.0 - error

            temp.append(score)
        
        mean = np.array(temp).mean()
        STD = np.array(temp).std()
        m.append(mean)
        std.append(STD)
    title = modeltype + " classification. 5 FoldsCV for " + hyperparameterLabel
    if scale:
        title += " (scaled input)"
    MeanSquareErrorPlot(title, hyperparameterLabel + " values", hyperparameters, m, std, yLabel = yLabel)
    return m, std


def TestValidationAccuracy(ModelType, dataset, c = None, K = None, scaleInput = True):
    print("Getting Validation accuracy for " + ModelType)
    x = np.array(dataset.trainX)
    y = np.array(dataset.trainY)
    if scaleInput:
        x = np.array(dataset.trainXScaled)

    
    if ModelType == "KNN":
        model = KNeighborsClassifier(n_neighbors = K)
    elif ModelType == "SVC":
        model = SVC(C = 1/(2 * c), probability=True)
    elif ModelType == "LinearSVCL1":
        model = LinearSVC(penalty='l1', loss='squared_hinge',C = 1/(2 * c), dual="auto")
    elif ModelType == "LinearSVCL2":
        model = LinearSVC(penalty='l2', C = 1/(2 * c),max_iter=1000)
    y = dataset.trainY
    model.fit(x,y)

    yPred = model.predict(dataset.validateXScaled)
    yActual = dataset.validateY
    # Compare the prediction to the actual
    error = np.mean( yPred != yActual )
    score = 1.0 - error
    
    print("Validation Accuracy for " + ModelType, ":", score)


class Dataset:
    def __init__(self, file,splitPercentage = .9, Debug = True):
        df = pd.read_json(file)
        
        numRowsTrain = int(splitPercentage * df.shape[0])
        numRowsValidate = df.shape[0] - numRowsTrain
        #https://stackoverflow.com/questions/29576430/shuffle-dataframe-rows
        df = shuffle(df, random_state = 0)
        df.reset_index(inplace=True, drop=True)

        if Debug:
            print(df.head())
            print(df.info())

        self.X = df.iloc[:,1:]
        self.y = df.iloc[:,0]
        self.ColumnNames = df.columns[1:]
        # https://scikit-learn.org/stable/modules/preprocessing.html
        scaler = preprocessing.StandardScaler().fit(self.X)
        self.scaler = scaler
        self.XScaled = scaler.transform(self.X)
        
        self.trainX = self.X.iloc[:numRowsTrain]
        self.validateX = self.X.iloc[numRowsTrain:]
        
        self.trainXScaled = scaler.transform(self.trainX)
        self.validateXScaled = scaler.transform(self.validateX)
        
        self.trainY = self.y.iloc[:numRowsTrain]
        self.validateY = self.y.iloc[numRowsTrain:]

class MLModel:
    def __init__(self):
        self.thetas = []
        self.type = None
        self.yPred = None
        self.model = None
        self.scaler = None

    def AssignModelAndHyperParameters(self, ModelType, c = None, K = None):
        assert(self.type == None and ModelType in ["KNN", "SVC", "LinearSVCL1", "LinearSVCL2"])
        self.type = ModelType
        if ModelType == "KNN":
            self.model = KNeighborsClassifier(n_neighbors = K)
            self.K = K
        elif ModelType == "SVC":
            self.model = SVC(C = 1/(2 * c), probability=True)
            self.C = 1/(2 * c)
        elif ModelType == "LinearSVCL1":
            self.model = LinearSVC(penalty='l1', loss='squared_hinge', C = 1/(2 * c), dual="auto")
            self.C = 1/(2 * c)
        elif ModelType == "LinearSVCL2":
            self.model = LinearSVC(penalty='l2', C = 1/(2 * c),max_iter=1000)
            self.C = 1/(2 * c)
            #assert (False)
        # print("Fitting " + self.type)

    def TrainModel(self, dataset, scaleInput = True):
        assert(self.model != None and self.type != None)
        print("Fitting " + self.type)
        x = dataset.X.values
        y = dataset.y
        self.isScaled = False
        if scaleInput:
            self.isScaled = True
            x = np.array(dataset.XScaled)
            self.scaler = dataset.scaler

        self.model.fit(x,y)

    def Predict(self, xParam, scale = False, validate = False):
        assert self.type != None
        x = xParam
        if scale and self.isScaled == True:
            x = np.array(self.scaler.transform(x))

        yPred = self.model.predict(x)
        return yPred
    
    # Expects a single item to be predicted on with optimizations
    def PredictOptimized(self, xParam, scale = False):
        assert self.type != None
        x = xParam
        if scale and self.isScaled == True:
            x = np.array(self.scaler.transform(x))
        
        yPred = self.model.predict(x)
        return yPred
    
    
    def LoadModel(self, path, mType, dataset, scale = True):
        print("Loading model", path)
        self.type = mType
        self.model = load(path)
        if scale:
            self.isScaled = True
            self.scaler = dataset.scaler

    # https://scikit-learn.org/stable/model_persistence.html
    def ExportModel(self, path = "./", prefixName = ""):
        targetPath = os.path.join(path, prefixName + "Model" + self.type +".joblib")
        print("Saving model", targetPath)
        dump(self.model, targetPath) 