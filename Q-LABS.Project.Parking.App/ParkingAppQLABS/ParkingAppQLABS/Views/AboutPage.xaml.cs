using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParkingAppQLABS.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
	{
		public AboutPage ()
		{
            InitializeComponent ();

		    AboutHeaderLabel.FontSize = 23;
              AboutHeaderLabel.TextColor = Color.DarkBlue;
              AboutHeaderLabel.Text = "About the 'Parking Solution'.";
		    AboutBodyLabel.Text = "Imagine, you drive to the university or travel by public transport and then look for a place at the university to work quietly. "+
		                          "Wouldn’t it be nice to know how crowded it is at the moment of arrival? Or rather at the time of departure? Or even days in advance? "+
		                          "With cloud services, we nowadays have a low-threshold use of smart services to make predictions. "+
		                          "For this you need data, IoT is often a good solution here. You also want to bring this to the end user in a pleasant and efficient way."+
		                          "\n\n"+
		                          "We are ilionx, a medium-sized IT consultancy company with more than 850 employees spread over 7 offices across the Netherlands: "+
		                          "Maastricht, Eindhoven, Amsterdam, Utrecht, Groningen, Zwolle and Groningen. "+
		                          "Each branch is divided into units and we have different specializations and knowledge: "+
		                          "Java, .NET, cloud computing, business analytics, digital experience, dev-operations, interactive marketing and security. "+
		                          "Open, collegial and informal are characteristics that describe the culture of ilionx."+
		                          "\n\n"+
		                          "Because of all our specializations, we founded Q-LABS in 2018 to bring together the various departments and competences "+
		                          "to realize smart and innovative solutions. Due to the parking problem at our office in Maastricht, the 'parking solution' was created. "+
		                          "A product where an end-2-end solution is built, where the competences BI, infra and development come together. "+
		                          "Parts and techniques that we have used include Python-scripts to read camera images, multiple smart devices (IoT), cloud services (Azure), "+
		                          "C# development, object recognition and a Xamarin app."+
		                          "\n\n"+
		                          "Q-LABS has recently become open-source, so if you are interested in contributing to one of the above mentioned knowledge areas while enjoying "+
		                          "a beer and pizza: you are welcome at our office in Maastricht!"+
		                          "\n\n"+
		                          "You can find us on github:";
		    AboutLinkLabel.Text = "https://github.com/ilionx";
              var tapGestureRecognizer = new TapGestureRecognizer();
		    tapGestureRecognizer.Tapped += (s, e) => {
		        Device.OpenUri(new Uri(AboutLinkLabel.Text));
		    };
		    AboutLinkLabel.GestureRecognizers.Add(tapGestureRecognizer);
		    AboutLinkLabel.TextColor = Color.Blue;
		    AboutFootLabel.Text = "Find the projects with prefix 'Qlabs-'.\n\n";
		}
    }
}