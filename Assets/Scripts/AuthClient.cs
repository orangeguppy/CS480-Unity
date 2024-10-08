using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class AuthClient : MonoBehaviour
{
    private static SceneNav sceneNav;
    // First get the fields that contain the username and password
    private static TMP_InputField username;
    private static TMP_InputField password;

    public static IEnumerator Login()
    {
        username = GameObject.FindWithTag("Username").GetComponent<TMP_InputField>();
        password = GameObject.FindWithTag("Password").GetComponent<TMP_InputField>();
        PopUpController popUpController = FindObjectOfType<PopUpController>();
    
        if (sceneNav == null)
        {
            sceneNav = GameObject.Find("Canvas").GetComponent<SceneNav>();
        }

        // Use UnityWebRequest for HTTP requests
        var formData = new WWWForm();
        formData.AddField("username", username.text);
        formData.AddField("password", password.text);

        // Save the email for autofilling
        PlayerPrefs.SetString("Email", username.text);

        using (UnityWebRequest request = UnityWebRequest.Post("http://127.0.0.1:8000/auth/login/token", formData))
        {
            Debug.Log("Sending request now");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                popUpController.ShowPopup("green", "Success", "Login successful!");
                Debug.Log("Success");

                string jsonResponse = request.downloadHandler.text;
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(jsonResponse);
                Debug.Log("Token: " + response.access_token);
                Debug.Log("User ID: " + response.token_type);
                // Save the access token to PlayerPrefs
                PlayerPrefs.SetString("AccessToken", response.access_token);

                sceneNav.loadMainMenu();
            }
            else if (request.responseCode == 403)
            {
                string res = request.downloadHandler.text;
                HTTPResponse httpRes = JsonUtility.FromJson<HTTPResponse>(res);
                res = httpRes.detail;
                Debug.LogError($"Login failed: {request.downloadHandler.text}");
                popUpController.ShowPopup("red", "Error", res);
                SceneController sceneController = GameObject.Find("ButtonController").GetComponent<SceneController>();
                GameObject loginPage = GameObject.Find("SignInPage");
                GameObject emailVerificationPage = sceneController.verifyEmailAddrPage;
                loginPage.SetActive(false);
                emailVerificationPage.SetActive(true);
            }
            else
            {
                string res = request.downloadHandler.text;
                HTTPResponse httpRes = JsonUtility.FromJson<HTTPResponse>(res);
                res = httpRes.detail;
                Debug.LogError($"Login failed: {request.downloadHandler.text}");
                popUpController.ShowPopup("red", "Error", res);
            }
        }
    }
}