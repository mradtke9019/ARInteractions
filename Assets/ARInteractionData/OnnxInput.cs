/*using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Onnx;
using System;
namespace Assets.ARInteractionData
{
    public class OnnxInput
    {
        public int Hand { get; set; }
        public float IndexCurl { get; set; }
        public float MiddleCurl { get; set; }
        public float RingCurl { get; set; }
        public float PinkyCurl { get; set; }
        public float ThumbCurl { get; set; }
        public int NoneXRotation { get; set; }
        public int NoneYRotation { get; set; }
        public int NoneZRotation { get; set; }
        public int NoneXPosition { get; set; }
        public int NoneYPosition { get; set; }
        public int NoneZPosition { get; set; }
        public int NoneXForward { get; set; }
        public int NoneYForward { get; set; }
        public int NoneZForward { get; set; }
        public int NoneXUp { get; set; }
        public int NoneYUp { get; set; }
        public int NoneZUp { get; set; }
        public int NoneXRight { get; set; }
        public int NoneYRight { get; set; }
        public int NoneZRight { get; set; }
        public int NoneXLocalRotation { get; set; }
        public int NoneYLocalRotation { get; set; }
        public int NoneZLocalRotation { get; set; }
        public int NoneXLocalPosition { get; set; }
        public int NoneYLocalPosition { get; set; }
        public int NoneZLocalPosition { get; set; }
        public float WristXRotation { get; set; }
        public float WristYRotation { get; set; }
        public float WristZRotation { get; set; }
        public float WristXPosition { get; set; }
        public float WristYPosition { get; set; }
        public float WristZPosition { get; set; }
        public float WristXForward { get; set; }
        public float WristYForward { get; set; }
        public float WristZForward { get; set; }
        public float WristXUp { get; set; }
        public float WristYUp { get; set; }
        public float WristZUp { get; set; }
        public float WristXRight { get; set; }
        public float WristYRight { get; set; }
        public float WristZRight { get; set; }
        public float WristXLocalRotation { get; set; }
        public float WristYLocalRotation { get; set; }
        public float WristZLocalRotation { get; set; }
        public float WristXLocalPosition { get; set; }
        public float WristYLocalPosition { get; set; }
        public float WristZLocalPosition { get; set; }
        public float PalmXRotation { get; set; }
        public float PalmYRotation { get; set; }
        public float PalmZRotation { get; set; }
        public float PalmXPosition { get; set; }
        public float PalmYPosition { get; set; }
        public float PalmZPosition { get; set; }
        public float PalmXForward { get; set; }
        public float PalmYForward { get; set; }
        public float PalmZForward { get; set; }
        public float PalmXUp { get; set; }
        public float PalmYUp { get; set; }
        public float PalmZUp { get; set; }
        public float PalmXRight { get; set; }
        public float PalmYRight { get; set; }
        public float PalmZRight { get; set; }
        public float PalmXLocalRotation { get; set; }
        public float PalmYLocalRotation { get; set; }
        public float PalmZLocalRotation { get; set; }
        public float PalmXLocalPosition { get; set; }
        public float PalmYLocalPosition { get; set; }
        public float PalmZLocalPosition { get; set; }
        public float ThumbMetacarpalJointXRotation { get; set; }
        public float ThumbMetacarpalJointYRotation { get; set; }
        public float ThumbMetacarpalJointZRotation { get; set; }
        public float ThumbMetacarpalJointXPosition { get; set; }
        public float ThumbMetacarpalJointYPosition { get; set; }
        public float ThumbMetacarpalJointZPosition { get; set; }
        public float ThumbMetacarpalJointXForward { get; set; }
        public float ThumbMetacarpalJointYForward { get; set; }
        public float ThumbMetacarpalJointZForward { get; set; }
        public float ThumbMetacarpalJointXUp { get; set; }
        public float ThumbMetacarpalJointYUp { get; set; }
        public float ThumbMetacarpalJointZUp { get; set; }
        public float ThumbMetacarpalJointXRight { get; set; }
        public float ThumbMetacarpalJointYRight { get; set; }
        public float ThumbMetacarpalJointZRight { get; set; }
        public float ThumbMetacarpalJointXLocalRotation { get; set; }
        public float ThumbMetacarpalJointYLocalRotation { get; set; }
        public float ThumbMetacarpalJointZLocalRotation { get; set; }
        public float ThumbMetacarpalJointXLocalPosition { get; set; }
        public float ThumbMetacarpalJointYLocalPosition { get; set; }
        public float ThumbMetacarpalJointZLocalPosition { get; set; }
        public float ThumbProximalJointXRotation { get; set; }
        public float ThumbProximalJointYRotation { get; set; }
        public float ThumbProximalJointZRotation { get; set; }
        public float ThumbProximalJointXPosition { get; set; }
        public float ThumbProximalJointYPosition { get; set; }
        public float ThumbProximalJointZPosition { get; set; }
        public float ThumbProximalJointXForward { get; set; }
        public float ThumbProximalJointYForward { get; set; }
        public float ThumbProximalJointZForward { get; set; }
        public float ThumbProximalJointXUp { get; set; }
        public float ThumbProximalJointYUp { get; set; }
        public float ThumbProximalJointZUp { get; set; }
        public float ThumbProximalJointXRight { get; set; }
        public float ThumbProximalJointYRight { get; set; }
        public float ThumbProximalJointZRight { get; set; }
        public float ThumbProximalJointXLocalRotation { get; set; }
        public float ThumbProximalJointYLocalRotation { get; set; }
        public float ThumbProximalJointZLocalRotation { get; set; }
        public float ThumbProximalJointXLocalPosition { get; set; }
        public float ThumbProximalJointYLocalPosition { get; set; }
        public float ThumbProximalJointZLocalPosition { get; set; }
        public float ThumbDistalJointXRotation { get; set; }
        public float ThumbDistalJointYRotation { get; set; }
        public float ThumbDistalJointZRotation { get; set; }
        public float ThumbDistalJointXPosition { get; set; }
        public float ThumbDistalJointYPosition { get; set; }
        public float ThumbDistalJointZPosition { get; set; }
        public float ThumbDistalJointXForward { get; set; }
        public float ThumbDistalJointYForward { get; set; }
        public float ThumbDistalJointZForward { get; set; }
        public float ThumbDistalJointXUp { get; set; }
        public float ThumbDistalJointYUp { get; set; }
        public float ThumbDistalJointZUp { get; set; }
        public float ThumbDistalJointXRight { get; set; }
        public float ThumbDistalJointYRight { get; set; }
        public float ThumbDistalJointZRight { get; set; }
        public float ThumbDistalJointXLocalRotation { get; set; }
        public float ThumbDistalJointYLocalRotation { get; set; }
        public float ThumbDistalJointZLocalRotation { get; set; }
        public float ThumbDistalJointXLocalPosition { get; set; }
        public float ThumbDistalJointYLocalPosition { get; set; }
        public float ThumbDistalJointZLocalPosition { get; set; }
        public float ThumbTipXRotation { get; set; }
        public float ThumbTipYRotation { get; set; }
        public float ThumbTipZRotation { get; set; }
        public float ThumbTipXPosition { get; set; }
        public float ThumbTipYPosition { get; set; }
        public float ThumbTipZPosition { get; set; }
        public float ThumbTipXForward { get; set; }
        public float ThumbTipYForward { get; set; }
        public float ThumbTipZForward { get; set; }
        public float ThumbTipXUp { get; set; }
        public float ThumbTipYUp { get; set; }
        public float ThumbTipZUp { get; set; }
        public float ThumbTipXRight { get; set; }
        public float ThumbTipYRight { get; set; }
        public float ThumbTipZRight { get; set; }
        public float ThumbTipXLocalRotation { get; set; }
        public float ThumbTipYLocalRotation { get; set; }
        public float ThumbTipZLocalRotation { get; set; }
        public float ThumbTipXLocalPosition { get; set; }
        public float ThumbTipYLocalPosition { get; set; }
        public float ThumbTipZLocalPosition { get; set; }
        public float IndexMetacarpalXRotation { get; set; }
        public float IndexMetacarpalYRotation { get; set; }
        public float IndexMetacarpalZRotation { get; set; }
        public float IndexMetacarpalXPosition { get; set; }
        public float IndexMetacarpalYPosition { get; set; }
        public float IndexMetacarpalZPosition { get; set; }
        public float IndexMetacarpalXForward { get; set; }
        public float IndexMetacarpalYForward { get; set; }
        public float IndexMetacarpalZForward { get; set; }
        public float IndexMetacarpalXUp { get; set; }
        public float IndexMetacarpalYUp { get; set; }
        public float IndexMetacarpalZUp { get; set; }
        public float IndexMetacarpalXRight { get; set; }
        public float IndexMetacarpalYRight { get; set; }
        public float IndexMetacarpalZRight { get; set; }
        public float IndexMetacarpalXLocalRotation { get; set; }
        public float IndexMetacarpalYLocalRotation { get; set; }
        public float IndexMetacarpalZLocalRotation { get; set; }
        public float IndexMetacarpalXLocalPosition { get; set; }
        public float IndexMetacarpalYLocalPosition { get; set; }
        public float IndexMetacarpalZLocalPosition { get; set; }
        public float IndexKnuckleXRotation { get; set; }
        public float IndexKnuckleYRotation { get; set; }
        public float IndexKnuckleZRotation { get; set; }
        public float IndexKnuckleXPosition { get; set; }
        public float IndexKnuckleYPosition { get; set; }
        public float IndexKnuckleZPosition { get; set; }
        public float IndexKnuckleXForward { get; set; }
        public float IndexKnuckleYForward { get; set; }
        public float IndexKnuckleZForward { get; set; }
        public float IndexKnuckleXUp { get; set; }
        public float IndexKnuckleYUp { get; set; }
        public float IndexKnuckleZUp { get; set; }
        public float IndexKnuckleXRight { get; set; }
        public float IndexKnuckleYRight { get; set; }
        public float IndexKnuckleZRight { get; set; }
        public float IndexKnuckleXLocalRotation { get; set; }
        public float IndexKnuckleYLocalRotation { get; set; }
        public float IndexKnuckleZLocalRotation { get; set; }
        public float IndexKnuckleXLocalPosition { get; set; }
        public float IndexKnuckleYLocalPosition { get; set; }
        public float IndexKnuckleZLocalPosition { get; set; }
        public float IndexMiddleJointXRotation { get; set; }
        public float IndexMiddleJointYRotation { get; set; }
        public float IndexMiddleJointZRotation { get; set; }
        public float IndexMiddleJointXPosition { get; set; }
        public float IndexMiddleJointYPosition { get; set; }
        public float IndexMiddleJointZPosition { get; set; }
        public float IndexMiddleJointXForward { get; set; }
        public float IndexMiddleJointYForward { get; set; }
        public float IndexMiddleJointZForward { get; set; }
        public float IndexMiddleJointXUp { get; set; }
        public float IndexMiddleJointYUp { get; set; }
        public float IndexMiddleJointZUp { get; set; }
        public float IndexMiddleJointXRight { get; set; }
        public float IndexMiddleJointYRight { get; set; }
        public float IndexMiddleJointZRight { get; set; }
        public float IndexMiddleJointXLocalRotation { get; set; }
        public float IndexMiddleJointYLocalRotation { get; set; }
        public float IndexMiddleJointZLocalRotation { get; set; }
        public float IndexMiddleJointXLocalPosition { get; set; }
        public float IndexMiddleJointYLocalPosition { get; set; }
        public float IndexMiddleJointZLocalPosition { get; set; }
        public float IndexDistalJointXRotation { get; set; }
        public float IndexDistalJointYRotation { get; set; }
        public float IndexDistalJointZRotation { get; set; }
        public float IndexDistalJointXPosition { get; set; }
        public float IndexDistalJointYPosition { get; set; }
        public float IndexDistalJointZPosition { get; set; }
        public float IndexDistalJointXForward { get; set; }
        public float IndexDistalJointYForward { get; set; }
        public float IndexDistalJointZForward { get; set; }
        public float IndexDistalJointXUp { get; set; }
        public float IndexDistalJointYUp { get; set; }
        public float IndexDistalJointZUp { get; set; }
        public float IndexDistalJointXRight { get; set; }
        public float IndexDistalJointYRight { get; set; }
        public float IndexDistalJointZRight { get; set; }
        public float IndexDistalJointXLocalRotation { get; set; }
        public float IndexDistalJointYLocalRotation { get; set; }
        public float IndexDistalJointZLocalRotation { get; set; }
        public float IndexDistalJointXLocalPosition { get; set; }
        public float IndexDistalJointYLocalPosition { get; set; }
        public float IndexDistalJointZLocalPosition { get; set; }
        public float IndexTipXRotation { get; set; }
        public float IndexTipYRotation { get; set; }
        public float IndexTipZRotation { get; set; }
        public float IndexTipXPosition { get; set; }
        public float IndexTipYPosition { get; set; }
        public float IndexTipZPosition { get; set; }
        public float IndexTipXForward { get; set; }
        public float IndexTipYForward { get; set; }
        public float IndexTipZForward { get; set; }
        public float IndexTipXUp { get; set; }
        public float IndexTipYUp { get; set; }
        public float IndexTipZUp { get; set; }
        public float IndexTipXRight { get; set; }
        public float IndexTipYRight { get; set; }
        public float IndexTipZRight { get; set; }
        public float IndexTipXLocalRotation { get; set; }
        public float IndexTipYLocalRotation { get; set; }
        public float IndexTipZLocalRotation { get; set; }
        public float IndexTipXLocalPosition { get; set; }
        public float IndexTipYLocalPosition { get; set; }
        public float IndexTipZLocalPosition { get; set; }
        public float MiddleMetacarpalXRotation { get; set; }
        public float MiddleMetacarpalYRotation { get; set; }
        public float MiddleMetacarpalZRotation { get; set; }
        public float MiddleMetacarpalXPosition { get; set; }
        public float MiddleMetacarpalYPosition { get; set; }
        public float MiddleMetacarpalZPosition { get; set; }
        public float MiddleMetacarpalXForward { get; set; }
        public float MiddleMetacarpalYForward { get; set; }
        public float MiddleMetacarpalZForward { get; set; }
        public float MiddleMetacarpalXUp { get; set; }
        public float MiddleMetacarpalYUp { get; set; }
        public float MiddleMetacarpalZUp { get; set; }
        public float MiddleMetacarpalXRight { get; set; }
        public float MiddleMetacarpalYRight { get; set; }
        public float MiddleMetacarpalZRight { get; set; }
        public float MiddleMetacarpalXLocalRotation { get; set; }
        public float MiddleMetacarpalYLocalRotation { get; set; }
        public float MiddleMetacarpalZLocalRotation { get; set; }
        public float MiddleMetacarpalXLocalPosition { get; set; }
        public float MiddleMetacarpalYLocalPosition { get; set; }
        public float MiddleMetacarpalZLocalPosition { get; set; }
        public float MiddleKnuckleXRotation { get; set; }
        public float MiddleKnuckleYRotation { get; set; }
        public float MiddleKnuckleZRotation { get; set; }
        public float MiddleKnuckleXPosition { get; set; }
        public float MiddleKnuckleYPosition { get; set; }
        public float MiddleKnuckleZPosition { get; set; }
        public float MiddleKnuckleXForward { get; set; }
        public float MiddleKnuckleYForward { get; set; }
        public float MiddleKnuckleZForward { get; set; }
        public float MiddleKnuckleXUp { get; set; }
        public float MiddleKnuckleYUp { get; set; }
        public float MiddleKnuckleZUp { get; set; }
        public float MiddleKnuckleXRight { get; set; }
        public float MiddleKnuckleYRight { get; set; }
        public float MiddleKnuckleZRight { get; set; }
        public float MiddleKnuckleXLocalRotation { get; set; }
        public float MiddleKnuckleYLocalRotation { get; set; }
        public float MiddleKnuckleZLocalRotation { get; set; }
        public float MiddleKnuckleXLocalPosition { get; set; }
        public float MiddleKnuckleYLocalPosition { get; set; }
        public float MiddleKnuckleZLocalPosition { get; set; }
        public float MiddleMiddleJointXRotation { get; set; }
        public float MiddleMiddleJointYRotation { get; set; }
        public float MiddleMiddleJointZRotation { get; set; }
        public float MiddleMiddleJointXPosition { get; set; }
        public float MiddleMiddleJointYPosition { get; set; }
        public float MiddleMiddleJointZPosition { get; set; }
        public float MiddleMiddleJointXForward { get; set; }
        public float MiddleMiddleJointYForward { get; set; }
        public float MiddleMiddleJointZForward { get; set; }
        public float MiddleMiddleJointXUp { get; set; }
        public float MiddleMiddleJointYUp { get; set; }
        public float MiddleMiddleJointZUp { get; set; }
        public float MiddleMiddleJointXRight { get; set; }
        public float MiddleMiddleJointYRight { get; set; }
        public float MiddleMiddleJointZRight { get; set; }
        public float MiddleMiddleJointXLocalRotation { get; set; }
        public float MiddleMiddleJointYLocalRotation { get; set; }
        public float MiddleMiddleJointZLocalRotation { get; set; }
        public float MiddleMiddleJointXLocalPosition { get; set; }
        public float MiddleMiddleJointYLocalPosition { get; set; }
        public float MiddleMiddleJointZLocalPosition { get; set; }
        public float MiddleDistalJointXRotation { get; set; }
        public float MiddleDistalJointYRotation { get; set; }
        public float MiddleDistalJointZRotation { get; set; }
        public float MiddleDistalJointXPosition { get; set; }
        public float MiddleDistalJointYPosition { get; set; }
        public float MiddleDistalJointZPosition { get; set; }
        public float MiddleDistalJointXForward { get; set; }
        public float MiddleDistalJointYForward { get; set; }
        public float MiddleDistalJointZForward { get; set; }
        public float MiddleDistalJointXUp { get; set; }
        public float MiddleDistalJointYUp { get; set; }
        public float MiddleDistalJointZUp { get; set; }
        public float MiddleDistalJointXRight { get; set; }
        public float MiddleDistalJointYRight { get; set; }
        public float MiddleDistalJointZRight { get; set; }
        public float MiddleDistalJointXLocalRotation { get; set; }
        public float MiddleDistalJointYLocalRotation { get; set; }
        public float MiddleDistalJointZLocalRotation { get; set; }
        public float MiddleDistalJointXLocalPosition { get; set; }
        public float MiddleDistalJointYLocalPosition { get; set; }
        public float MiddleDistalJointZLocalPosition { get; set; }
        public float MiddleTipXRotation { get; set; }
        public float MiddleTipYRotation { get; set; }
        public float MiddleTipZRotation { get; set; }
        public float MiddleTipXPosition { get; set; }
        public float MiddleTipYPosition { get; set; }
        public float MiddleTipZPosition { get; set; }
        public float MiddleTipXForward { get; set; }
        public float MiddleTipYForward { get; set; }
        public float MiddleTipZForward { get; set; }
        public float MiddleTipXUp { get; set; }
        public float MiddleTipYUp { get; set; }
        public float MiddleTipZUp { get; set; }
        public float MiddleTipXRight { get; set; }
        public float MiddleTipYRight { get; set; }
        public float MiddleTipZRight { get; set; }
        public float MiddleTipXLocalRotation { get; set; }
        public float MiddleTipYLocalRotation { get; set; }
        public float MiddleTipZLocalRotation { get; set; }
        public float MiddleTipXLocalPosition { get; set; }
        public float MiddleTipYLocalPosition { get; set; }
        public float MiddleTipZLocalPosition { get; set; }
        public float RingMetacarpalXRotation { get; set; }
        public float RingMetacarpalYRotation { get; set; }
        public float RingMetacarpalZRotation { get; set; }
        public float RingMetacarpalXPosition { get; set; }
        public float RingMetacarpalYPosition { get; set; }
        public float RingMetacarpalZPosition { get; set; }
        public float RingMetacarpalXForward { get; set; }
        public float RingMetacarpalYForward { get; set; }
        public float RingMetacarpalZForward { get; set; }
        public float RingMetacarpalXUp { get; set; }
        public float RingMetacarpalYUp { get; set; }
        public float RingMetacarpalZUp { get; set; }
        public float RingMetacarpalXRight { get; set; }
        public float RingMetacarpalYRight { get; set; }
        public float RingMetacarpalZRight { get; set; }
        public float RingMetacarpalXLocalRotation { get; set; }
        public float RingMetacarpalYLocalRotation { get; set; }
        public float RingMetacarpalZLocalRotation { get; set; }
        public float RingMetacarpalXLocalPosition { get; set; }
        public float RingMetacarpalYLocalPosition { get; set; }
        public float RingMetacarpalZLocalPosition { get; set; }
        public float RingKnuckleXRotation { get; set; }
        public float RingKnuckleYRotation { get; set; }
        public float RingKnuckleZRotation { get; set; }
        public float RingKnuckleXPosition { get; set; }
        public float RingKnuckleYPosition { get; set; }
        public float RingKnuckleZPosition { get; set; }
        public float RingKnuckleXForward { get; set; }
        public float RingKnuckleYForward { get; set; }
        public float RingKnuckleZForward { get; set; }
        public float RingKnuckleXUp { get; set; }
        public float RingKnuckleYUp { get; set; }
        public float RingKnuckleZUp { get; set; }
        public float RingKnuckleXRight { get; set; }
        public float RingKnuckleYRight { get; set; }
        public float RingKnuckleZRight { get; set; }
        public float RingKnuckleXLocalRotation { get; set; }
        public float RingKnuckleYLocalRotation { get; set; }
        public float RingKnuckleZLocalRotation { get; set; }
        public float RingKnuckleXLocalPosition { get; set; }
        public float RingKnuckleYLocalPosition { get; set; }
        public float RingKnuckleZLocalPosition { get; set; }
        public float RingMiddleJointXRotation { get; set; }
        public float RingMiddleJointYRotation { get; set; }
        public float RingMiddleJointZRotation { get; set; }
        public float RingMiddleJointXPosition { get; set; }
        public float RingMiddleJointYPosition { get; set; }
        public float RingMiddleJointZPosition { get; set; }
        public float RingMiddleJointXForward { get; set; }
        public float RingMiddleJointYForward { get; set; }
        public float RingMiddleJointZForward { get; set; }
        public float RingMiddleJointXUp { get; set; }
        public float RingMiddleJointYUp { get; set; }
        public float RingMiddleJointZUp { get; set; }
        public float RingMiddleJointXRight { get; set; }
        public float RingMiddleJointYRight { get; set; }
        public float RingMiddleJointZRight { get; set; }
        public float RingMiddleJointXLocalRotation { get; set; }
        public float RingMiddleJointYLocalRotation { get; set; }
        public float RingMiddleJointZLocalRotation { get; set; }
        public float RingMiddleJointXLocalPosition { get; set; }
        public float RingMiddleJointYLocalPosition { get; set; }
        public float RingMiddleJointZLocalPosition { get; set; }
        public float RingDistalJointXRotation { get; set; }
        public float RingDistalJointYRotation { get; set; }
        public float RingDistalJointZRotation { get; set; }
        public float RingDistalJointXPosition { get; set; }
        public float RingDistalJointYPosition { get; set; }
        public float RingDistalJointZPosition { get; set; }
        public float RingDistalJointXForward { get; set; }
        public float RingDistalJointYForward { get; set; }
        public float RingDistalJointZForward { get; set; }
        public float RingDistalJointXUp { get; set; }
        public float RingDistalJointYUp { get; set; }
        public float RingDistalJointZUp { get; set; }
        public float RingDistalJointXRight { get; set; }
        public float RingDistalJointYRight { get; set; }
        public float RingDistalJointZRight { get; set; }
        public float RingDistalJointXLocalRotation { get; set; }
        public float RingDistalJointYLocalRotation { get; set; }
        public float RingDistalJointZLocalRotation { get; set; }
        public float RingDistalJointXLocalPosition { get; set; }
        public float RingDistalJointYLocalPosition { get; set; }
        public float RingDistalJointZLocalPosition { get; set; }
        public float RingTipXRotation { get; set; }
        public float RingTipYRotation { get; set; }
        public float RingTipZRotation { get; set; }
        public float RingTipXPosition { get; set; }
        public float RingTipYPosition { get; set; }
        public float RingTipZPosition { get; set; }
        public float RingTipXForward { get; set; }
        public float RingTipYForward { get; set; }
        public float RingTipZForward { get; set; }
        public float RingTipXUp { get; set; }
        public float RingTipYUp { get; set; }
        public float RingTipZUp { get; set; }
        public float RingTipXRight { get; set; }
        public float RingTipYRight { get; set; }
        public float RingTipZRight { get; set; }
        public float RingTipXLocalRotation { get; set; }
        public float RingTipYLocalRotation { get; set; }
        public float RingTipZLocalRotation { get; set; }
        public float RingTipXLocalPosition { get; set; }
        public float RingTipYLocalPosition { get; set; }
        public float RingTipZLocalPosition { get; set; }
        public float PinkyMetacarpalXRotation { get; set; }
        public float PinkyMetacarpalYRotation { get; set; }
        public float PinkyMetacarpalZRotation { get; set; }
        public float PinkyMetacarpalXPosition { get; set; }
        public float PinkyMetacarpalYPosition { get; set; }
        public float PinkyMetacarpalZPosition { get; set; }
        public float PinkyMetacarpalXForward { get; set; }
        public float PinkyMetacarpalYForward { get; set; }
        public float PinkyMetacarpalZForward { get; set; }
        public float PinkyMetacarpalXUp { get; set; }
        public float PinkyMetacarpalYUp { get; set; }
        public float PinkyMetacarpalZUp { get; set; }
        public float PinkyMetacarpalXRight { get; set; }
        public float PinkyMetacarpalYRight { get; set; }
        public float PinkyMetacarpalZRight { get; set; }
        public float PinkyMetacarpalXLocalRotation { get; set; }
        public float PinkyMetacarpalYLocalRotation { get; set; }
        public float PinkyMetacarpalZLocalRotation { get; set; }
        public float PinkyMetacarpalXLocalPosition { get; set; }
        public float PinkyMetacarpalYLocalPosition { get; set; }
        public float PinkyMetacarpalZLocalPosition { get; set; }
        public float PinkyKnuckleXRotation { get; set; }
        public float PinkyKnuckleYRotation { get; set; }
        public float PinkyKnuckleZRotation { get; set; }
        public float PinkyKnuckleXPosition { get; set; }
        public float PinkyKnuckleYPosition { get; set; }
        public float PinkyKnuckleZPosition { get; set; }
        public float PinkyKnuckleXForward { get; set; }
        public float PinkyKnuckleYForward { get; set; }
        public float PinkyKnuckleZForward { get; set; }
        public float PinkyKnuckleXUp { get; set; }
        public float PinkyKnuckleYUp { get; set; }
        public float PinkyKnuckleZUp { get; set; }
        public float PinkyKnuckleXRight { get; set; }
        public float PinkyKnuckleYRight { get; set; }
        public float PinkyKnuckleZRight { get; set; }
        public float PinkyKnuckleXLocalRotation { get; set; }
        public float PinkyKnuckleYLocalRotation { get; set; }
        public float PinkyKnuckleZLocalRotation { get; set; }
        public float PinkyKnuckleXLocalPosition { get; set; }
        public float PinkyKnuckleYLocalPosition { get; set; }
        public float PinkyKnuckleZLocalPosition { get; set; }
        public float PinkyMiddleJointXRotation { get; set; }
        public float PinkyMiddleJointYRotation { get; set; }
        public float PinkyMiddleJointZRotation { get; set; }
        public float PinkyMiddleJointXPosition { get; set; }
        public float PinkyMiddleJointYPosition { get; set; }
        public float PinkyMiddleJointZPosition { get; set; }
        public float PinkyMiddleJointXForward { get; set; }
        public float PinkyMiddleJointYForward { get; set; }
        public float PinkyMiddleJointZForward { get; set; }
        public float PinkyMiddleJointXUp { get; set; }
        public float PinkyMiddleJointYUp { get; set; }
        public float PinkyMiddleJointZUp { get; set; }
        public float PinkyMiddleJointXRight { get; set; }
        public float PinkyMiddleJointYRight { get; set; }
        public float PinkyMiddleJointZRight { get; set; }
        public float PinkyMiddleJointXLocalRotation { get; set; }
        public float PinkyMiddleJointYLocalRotation { get; set; }
        public float PinkyMiddleJointZLocalRotation { get; set; }
        public float PinkyMiddleJointXLocalPosition { get; set; }
        public float PinkyMiddleJointYLocalPosition { get; set; }
        public float PinkyMiddleJointZLocalPosition { get; set; }
        public float PinkyDistalJointXRotation { get; set; }
        public float PinkyDistalJointYRotation { get; set; }
        public float PinkyDistalJointZRotation { get; set; }
        public float PinkyDistalJointXPosition { get; set; }
        public float PinkyDistalJointYPosition { get; set; }
        public float PinkyDistalJointZPosition { get; set; }
        public float PinkyDistalJointXForward { get; set; }
        public float PinkyDistalJointYForward { get; set; }
        public float PinkyDistalJointZForward { get; set; }
        public float PinkyDistalJointXUp { get; set; }
        public float PinkyDistalJointYUp { get; set; }
        public float PinkyDistalJointZUp { get; set; }
        public float PinkyDistalJointXRight { get; set; }
        public float PinkyDistalJointYRight { get; set; }
        public float PinkyDistalJointZRight { get; set; }
        public float PinkyDistalJointXLocalRotation { get; set; }
        public float PinkyDistalJointYLocalRotation { get; set; }
        public float PinkyDistalJointZLocalRotation { get; set; }
        public float PinkyDistalJointXLocalPosition { get; set; }
        public float PinkyDistalJointYLocalPosition { get; set; }
        public float PinkyDistalJointZLocalPosition { get; set; }
        public float PinkyTipXRotation { get; set; }
        public float PinkyTipYRotation { get; set; }
        public float PinkyTipZRotation { get; set; }
        public float PinkyTipXPosition { get; set; }
        public float PinkyTipYPosition { get; set; }
        public float PinkyTipZPosition { get; set; }
        public float PinkyTipXForward { get; set; }
        public float PinkyTipYForward { get; set; }
        public float PinkyTipZForward { get; set; }
        public float PinkyTipXUp { get; set; }
        public float PinkyTipYUp { get; set; }
        public float PinkyTipZUp { get; set; }
        public float PinkyTipXRight { get; set; }
        public float PinkyTipYRight { get; set; }
        public float PinkyTipZRight { get; set; }
        public float PinkyTipXLocalRotation { get; set; }
        public float PinkyTipYLocalRotation { get; set; }
        public float PinkyTipZLocalRotation { get; set; }
        public float PinkyTipXLocalPosition { get; set; }
        public float PinkyTipYLocalPosition { get; set; }
        public float PinkyTipZLocalPosition { get; set; }
    }

}*/