/// <summary>
/// Copyright 2021 HGTP Capstone Team at the University of Utah: 
/// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
/// 
/// Licensed under the MIT license. Read the project readme for details.
/// </summary>

using Android.Database;
using Android.Provider;
using Android.Telephony;

namespace RevisedGestrApp.Droid.Services.Texts
{
    class SmsManagerWrapper : ISmsManager
    {
        private SmsManager smsManager;

        public SmsManagerWrapper(SmsManager smsManager)
        {
            this.smsManager = smsManager;
        }

        public void SendTextMessage(string number, string text)
        {

            this.smsManager.SendTextMessage(number, null, text, null, null);

        }

        public void ReadRecentText()
        {
            // Open user's sms inbox
            ICursor cursor = MainActivity.Instance.ContentResolver.Query(Android.Net.Uri.Parse("content://sms/inbox"), null, null, null);
            if (cursor.MoveToFirst()) // Let's get the data for the first message pointed to by cursor
                                      // (Should be the only message we need)
            {
                string sms_body = "";
                string address = "";
                for (int i = 0; i < cursor.ColumnCount; i++)
                {
                    switch (cursor.GetColumnName(i))
                    {
                        case "address":
                            string number = cursor.GetString(i);
                            string resolved_number = getContactbyPhoneNumber(number);
                            if (resolved_number == number)
                            {
                                if (number.Length >= 10)
                                {
                                    number = number.Substring(number.Length - 10);
                                    address = "(" + number.Substring(0, 3) + ") " + number.Substring(3, 3) + "-" + number.Substring(6, 4);
                                }
                                else
                                    address = string.Join(" ", number.ToCharArray());
                            }
                            else
                                address = resolved_number;
                            break;

                        case "body":
                            sms_body = cursor.GetString(i);
                            break;
                    }
                }
                var readAloud = new TextToAudio("Message from: " + address + ". " + sms_body);
            }
            cursor.Close();
        }

        /*
         * Based on Stackoverflow answer here: https://stackoverflow.com/a/45133911
         * By: Arifuzzaman Arif
         * */
        private string getContactbyPhoneNumber(string phoneNumber)
        {
            Android.Net.Uri uri = Android.Net.Uri.WithAppendedPath(ContactsContract.PhoneLookup.ContentFilterUri, Android.Net.Uri.Encode(phoneNumber));
            string[] projection = { ContactsContract.PhoneLookup.InterfaceConsts.DisplayName };
            ICursor cursor = MainActivity.Instance.ContentResolver.Query(uri, projection, null, null, null);

            if (cursor == null)
                return phoneNumber;
            else
            {
                string name = phoneNumber;
                try
                {
                    if (cursor.MoveToFirst())
                        name = cursor.GetString(cursor.GetColumnIndex(ContactsContract.PhoneLookup.InterfaceConsts.DisplayName));
                }
                finally
                {
                    cursor.Close();
                }
                return name;
            }
        }
    }
}
