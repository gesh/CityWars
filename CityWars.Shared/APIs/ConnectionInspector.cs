using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;
using System.Net.NetworkInformation;

namespace CityWars.APIs
{
    public class ConnectionInspector
    {
        public static bool IsOnline()
        {
            ConnectionProfile internetConnectionProfile = Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile();

            var request = (HttpWebRequest)WebRequest.Create("http://www.google.com/");

            bool isConnected = NetworkInterface.GetIsNetworkAvailable();

            //if (!request.HaveResponse)
            //{
            //    return false;
            //}
            if (internetConnectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess && isConnected)
            {
                if (internetConnectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.None || internetConnectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.LocalAccess)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

    }
}
