using Android.App;
using Android.OS;
using Android.Widget;
using System.Text;

namespace CodeLogic_Android
{
    [Activity(Label = "Application Unrecoverable State")]
    public class ApplicationUnrecoverableStateActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StringBuilder message = new StringBuilder();
            message.AppendLine("Congrats! You have found the ultra, super secret, unrecoverable application state!");
            message.AppendLine();
            message.AppendLine("You most likely found yourself here because you switched out of the game while an ad was showing. This is caused by an issue with MonoGame (#MonoGame, the framework this application was built upon). We at ArbitraryPixel Software sincerely apologize for this! Unfortunately, until the MonoGame team addresses this, there is no way to prevent, nor recover from, this issue. All we can do is recommend not task switching while an ad is showing. If and when a fix becomes available, we will issue an update as soon as possible to address this.");
            message.AppendLine();
            message.AppendLine("Please manually kill this application and restart. Also enjoy this trophy... because you only got here because you viewed an ad, which helps our development. You're a winner and we <3 you!");

            LinearLayout layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;
            layout.AddView(
                new TextView(this)
                {
                    Text = message.ToString()
                }
            );

            ImageView trophyImage = new ImageView(this);
            trophyImage.SetImageResource(Resource.Drawable.Trophy);
            layout.AddView(trophyImage);

            this.SetContentView(layout);
        }
    }
}