using XboxCtrlrInput;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Utils {

    // Creates a random point in a cicle's circumference
    public static Vector3 RandomPointInCircle(Vector3 center, float radius)
    {
        float randomAngle = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(randomAngle * Mathf.Deg2Rad);
        pos.z = center.z + radius * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        pos.y = center.y;

        return pos;
    }

    public static Vector3 RandomPoinInBox(Vector3 Center, Vector3 Size)
    {
        Vector3 pos = new Vector3
        {
            x = Center.x + Random.Range(-Size.x, Size.x),
            //pos.y = Center.y + Random.Range(-Size.y, Size.y);
            z = Center.z + Random.Range(-Size.z, Size.z)
        };
        return pos;
    }

    public static Options.Settings GetOptionsFromFile(string fileName)
    {
        Options.Settings dat;
        try
        {
            using (FileStream reader = new FileStream(fileName, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                dat = (Options.Settings)formatter.Deserialize(reader);
            }
        }catch(SerializationException ex)
        {
            dat = new Options.Settings();
        }

        return dat;
    }

    public static void WriteToFile(Options.Settings settings, string fileName)
    {
        using(FileStream writer = new FileStream(fileName, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(writer, settings);
        }
    }

    public static void SetObliqueness(float horizObl, float vertObl, Camera mainCam)
    {
        Matrix4x4 mat = mainCam.projectionMatrix;
        mat[0, 2] = horizObl;
        mat[1, 2] = vertObl;
        mainCam.projectionMatrix = mat;
    }

    public static bool AnyButtonAnyControll()
    {
        return XCI.GetButtonDown(XboxButton.A | XboxButton.B | XboxButton.DPadDown |
            XboxButton.DPadLeft | XboxButton.DPadRight | XboxButton.DPadUp | XboxButton.LeftBumper | XboxButton.LeftStick | XboxButton.RightBumper
            | XboxButton.RightStick | XboxButton.Start | XboxButton.X | XboxButton.Y, XboxController.Any);
    }
}