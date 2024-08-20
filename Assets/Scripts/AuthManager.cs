using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Collections;
using System.Threading.Tasks;
using static AuthManager;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference databaseReference;
    [SerializeField]
    public static string userId_str;

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;
    [SerializeField]
    private TMP_InputField _nameText;

    private string message;

    //  [Space(10)]


    private void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && emailLoginField.isFocused)
        {
            // Perform Tab Function 
            // Go to Next Line 
            // Also Ask Ramish where TAB Functionality is implemented so that we can implement it here. 
        }

        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            // Call the login button functionality
            LoginButton();
        }
    }
    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        // Call the Firebase auth signin function passing the email and password
        Task<AuthResult> LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        // Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            Debug.LogWarning("LoginTask encountered an exception"); // Added debug message

            // If there are errors, handle them
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;

            if (firebaseEx != null)
            {
                Debug.Log("FirebaseException encountered"); // Added debug message
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                switch (errorCode)
                {
                    case AuthError.MissingEmail:      // working
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:     // working
                        message = "Missing Password";
                        break;
                    case AuthError.WrongPassword:
                        message = "Wrong Password";
                        break;
                    case AuthError.InvalidRecipientEmail:     // not working
                        message = "Invalid Email";
                        break;
                    case AuthError.UserNotFound:         // not working
                        message = "Account does not exist";
                        break;
                    default:
                        message = "Invalid Email ID or Password";
                        break;
                }
                warningLoginText.text = message;
                Debug.Log("Login error: " + message); // Added debug message
            }
            else
            {
                warningLoginText.text = "An unknown error occurred.";
                Debug.LogError("Login failed with an unknown error."); // Added debug message
            }
        }
        else
        {

            // User is now logged in
            // Now get the result

            //  Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "Logged In";
            // UiManager._instance.ShowSplashScreen();

            User = LoginTask.Result.User;
            Debug.Log("Result.User UserId: " + User.UserId);
            userId_str = User.UserId;

            // FOR TESTING 
            SceneManager.LoadSceneAsync(1);
        }
        // yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);
        // yield return new WaitForSeconds(0.01f);
    }


    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            // If the username field is blank, show a warning
            warningRegisterText.text = "Missing Username";
            Debug.Log("Register failed: Missing Username");
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            // If the password does not match, show a warning
            warningRegisterText.text = "Password Does Not Match!";
            Debug.Log("Register failed: Password Does Not Match");
        }
        else
        {
            // Call the Firebase auth sign-in function passing the email and password
            Task<AuthResult> RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            // Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                // If there are errors, handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                    default:
                        message = "Unknown Error: " + firebaseEx.Message;
                        break;
                }
                warningRegisterText.text = message;
                Debug.Log("Register error: " + message);
            }
            else
            {
                // User has now been created
                // Now get the result
                User = RegisterTask.Result.User;

                if (User != null)
                {
                    // Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    // Call the Firebase auth update user profile function passing the profile with the username
                    Task ProfileTask = User.UpdateUserProfileAsync(profile);
                    // Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        // If there are errors, handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed!";
                        Debug.Log("Username set failed: " + firebaseEx.Message);
                    }
                    else
                    {
                        // Username is now set
                        // Now return to login screen
                        UiManager._instance.ShowLoginScreen();
                        warningRegisterText.text = "";
                        Debug.Log("Username set successfully and returning to login screen.");
                    }
                }
            }
        }
    }

    public void SaveData()
    {
        StartCoroutine(AddFieldInDb(_nameText.text));
    }
    IEnumerator AddFieldInDb(string field)
    {
        if (field != null || field != "")
        {
            var dbTask = databaseReference.Child("users").Child(User.UserId).Child("name").SetValueAsync(field);
            yield return new WaitUntil(predicate: () => dbTask.IsCompleted);
            if (dbTask.Exception != null)
            {
                Debug.LogError(message: $"failed to registere task with {dbTask.Exception}");
            }
            else
            {
                Debug.Log("Database updated successfully!");
            }
        }
    }

}
