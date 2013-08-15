using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spring.IO;
using Spring.Social.Dropbox.Api;
using Spring.Social.Dropbox.Connect;
using Spring.Social.OAuth1;

namespace FileUploader
{
    public class PictureUplouder
    {
        private const string ConsumerKey = "4t8yb4g3s76ssmz";
        private const string ConsumerSecret = "cniwjbgz6w78ul3";


        public static string LoadPicture(string path,string picName)
        {
            DropboxServiceProvider dropboxServiceProvider =
         new DropboxServiceProvider(ConsumerKey, ConsumerSecret, AccessLevel.AppFolder);

            OAuthToken oauthAccessToken = new OAuthToken("4a3ourja1xrknq2y", "hccgil4xmmsv5rt");

            IDropbox dropbox = dropboxServiceProvider.GetApi(oauthAccessToken.Value, oauthAccessToken.Secret);


            Entry uploadFileEntry = dropbox.UploadFileAsync(new FileResource(path), picName +".jpg").Result;

            DropboxLink sharedUrl = dropbox.GetMediaLinkAsync(uploadFileEntry.Path).Result;
            string link = sharedUrl.Url;
            return link;
        }
    }
}
