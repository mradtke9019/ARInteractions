/*using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Onnx;
using System;
namespace Assets.ARInteractionData
{
    public class OnnxInput
    {
        [ColumnName("vendor_id")]
        public string VendorId { get; set; }

        [ColumnName("rate_code"), OnnxMapType(typeof(Int64), typeof(Single))]
        public Int64 RateCode { get; set; }

        [ColumnName("passenger_count"), OnnxMapType(typeof(Int64), typeof(Single))]
        public Int64 PassengerCount { get; set; }

        [ColumnName("trip_time_in_secs"), OnnxMapType(typeof(Int64), typeof(Single))]
        public Int64 TripTimeInSecs { get; set; }

        [ColumnName("trip_distance")]
        public float TripDistance { get; set; }

        [ColumnName("payment_type")]
        public string PaymentType { get; set; }
    }
}*/