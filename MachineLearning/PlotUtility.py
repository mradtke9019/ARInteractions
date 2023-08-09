
import matplotlib.pyplot as plt


class ScatterData:
    def __init__(self,x,y,color,label,marker,alpha, annotations = None):
        self.x = x
        self.y = y
        self.color = color
        self.label = label
        self.marker = marker
        self.alpha = alpha
        self.annotations = annotations
class PlotData:
    def __init__(self,x,y,color,label,alpha):
        self.x = x
        self.y = y
        self.color = color
        self.label = label
        self.alpha = alpha
class HistogramData:
    def __init__(self,data,numBins):
        self.data = data
        self.numBins = numBins


def PlotHelper(title, xLabel, yLabel, scatterData = None, plotData = None, histogramData = None, OrderByX = False):
    fig = plt.figure()
    ax= plt.axes()
    ax.set_xlabel(xLabel)
    ax.set_ylabel(yLabel)
    ax.set_title(title, loc='left')
    # Rearrange the data to be in sequential order for a plot
    if OrderByX and plotData != None:
        for data in plotData:
            data.x,data.y = zip(*sorted(zip(data.x, data.y)))

    #(self,x,y,color,label,marker,alpha):
    if scatterData is not None:
        annotations = {}
        for data in scatterData:
            if data.color == None:
                ax.scatter(data.x,data.y, label = data.label, alpha = data.alpha, marker =data.marker)
            else:
                ax.scatter(data.x,data.y, label = data.label, color=data.color, alpha = data.alpha, marker =data.marker)
            # If annotations share same physical point in space, join them with a comma on the same line
            if(data.annotations != None):
                # annotations.append(annotation, (data.x[i], data.y[i]))
                for i, annotation in enumerate(data.annotations):
                    # ax.annotate(annotation, (data.x[i], data.y[i]))
                    key = tuple((data.x[i], data.y[i]))
                    if key not in annotations:
                        annotations[key] = annotation
                    else:
                        annotations[key] = annotations[key]  + ", " + annotation
                    # annotations.append({"text": annotation, "coord" : (data.x[i], data.y[i])})
        for coord in annotations:
            annotation = annotations[coord]
            offset = -0.04
            finalPlacement = coord
            even = coord[0] % 2 == 0
            if even == False:
                finalPlacement = (coord[0], coord[1]+ offset)
            else:
                finalPlacement = (coord[0], coord[1]- (offset/2.0))
            ax.annotate(annotation, finalPlacement, fontsize=8)

        

    if plotData is not None:
        for data in plotData:
            if data.color == None:
                ax.plot(data.x,data.y, label = data.label, alpha = data.alpha)
            else:
                ax.plot(data.x,data.y, label = data.label, color=data.color, alpha = data.alpha)
    
    if histogramData is not None:
        plt.hist(histogramData.data, bins=histogramData.numBins)
            
    plt.legend()
    plt.show()

def MeanSquareErrorPlot(title, xLabel, xVals, mse, std, yLabel = "Mean Squared Error"):
    fig = plt.figure() 
    ax = plt.axes()
    ax.set_title(title)
    plt.xlabel(xLabel)
    plt.ylabel(yLabel)   
    plt.errorbar(xVals, mse, yerr=std)
    plt.show()