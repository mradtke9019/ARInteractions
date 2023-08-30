# ARInteractions

## Abstract
The goal of this work is to understand the current state of interactions in augmented reality (AR), and propose an alternate approach to interaction in AR by utilizing modern software and hardware. The work presented seeks to answer the following questions: Can we predict the pose of the user's hand using the data gathered by the HoloLens 2, create unique combinations of those poses to be executed by the user as an alternate way to interact with the AR environment, and can we ensure that such gestures are usable. The work evaluates the proposed methodology by performing different experiments measuring the prediction accuracy of poses, overall success rate of gesture execution, and real time performance. The work concludes that this proposed methodology of driving gestural interaction from pose recognition is feasible.

### Technicals
This project is implemented in Unity version 2020.3.47.f1, and is designed to target the HoloLens 2 hardware. The bulk of the code is located in the Assets/Spellcaster folder within the file explorer. This program follows the paradigm of all interaction being driven by the Gesture Listener script. This script is attached to a single object within a scene and will need to be associated to all available gestures created within the system. Gestures are initiated from a prefab to create all the requirements and actions associated to a gesture. 

The program is designed to allow for an easy transition of source data if the HoloLens is no longer the targeted platform. Training data and features would need to be gathered and engineered for the new data source.

A flask server is ran from the MachineLearning app.py file. The AR Gesture recognition programw will communicate with this flask server over HTTP to make pose predictions based on the input data. This app.py file will hold the machine learning models trained on the data in SKLearn.
