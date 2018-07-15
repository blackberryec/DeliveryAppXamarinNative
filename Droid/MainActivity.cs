using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Microsoft.WindowsAzure.MobileServices;
using System.Linq;

namespace DeliveriesA.Droid
{
    [Activity(Label = "DeliveriesA", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        public static MobileServiceClient MobileService =
            new MobileServiceClient(
            "https://xamarinnativedeliveriesapp.azurewebsites.net"
        );

        EditText emailEditText, passwordEditText;
        Button signInButton, registerButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            emailEditText = FindViewById<EditText>(Resource.Id.emailEditText);
            passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
            signInButton = FindViewById<Button>(Resource.Id.signInButton);
            registerButton = FindViewById<Button>(Resource.Id.registerButton);

            signInButton.Click += SignInButton_Click;
            registerButton.Click += RegisterButton_Click;

        }

        private async void SignInButton_Click(object sender, System.EventArgs e)
        {
            var email = emailEditText.Text;
            var password = passwordEditText.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Toast.MakeText(this, "Email and password cannot null", ToastLength.Long).Show();
            }
            else
            {
                var user = (await MobileService.GetTable<User>().Where(u => u.Email == email && u.Password == password).ToListAsync()).FirstOrDefault();

                if (user.Password == password)
                {
                    Toast.MakeText(this, "Login successfull", ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(this, "Incorrect password", ToastLength.Long).Show();
                }
            }
        }

        void RegisterButton_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(this, typeof(RegisterActivity));
            intent.PutExtra("email", emailEditText.Text);
            StartActivity(intent);
        }

    }
}

