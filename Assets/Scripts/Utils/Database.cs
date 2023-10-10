using Firebase.Database;
using SimpleJSON;

public static class Database
{
    public static DatabaseReference dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    public static string url = "https://geebee-backend-85219523e92f.herokuapp.com";

    public static string FormatFirebaseData(string data)
    {
        if (string.IsNullOrEmpty(data))
            return "";

        string result = "{\"Items\":[";
        JSONNode root = JSONNode.Parse(data);

        for (int i = 0; i < root.Count; i++)
        {
            result += root[i].ToString();

            if (i < root.Count - 1)
                result += ",";
        }

        result += "]}";

        return result;
    }
}