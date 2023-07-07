public interface IData
{
    string ToJsonString();
    string ToFlaskParameter(string key = "data" , bool wrap = true);
}