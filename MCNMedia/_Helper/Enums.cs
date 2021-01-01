using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCNMedia_Dev._Helper
{
    public class Enums
    {


    }

    public enum UploadingAreas
    {
        ChurchProfileImage,
        Picture,
        Video,
        SlideShow,
        UserProfileImage,
        NewsLetter,
        Donation
    }

    public enum CameraType
    {
        All,
        AdminCamera,
        ClientCamera,
    }

    public enum Operation
    {
        Add,
        Update,
        Delete,
        Recording_Published_Request,
        Recording_Published,
        Recording_Started,
        Recording_Stopped
    }

    public enum Categories
    {
        Announcement,
        Camera,
        Church,
        Donate_Link,
        NewsLetter,
        Notice,
        Media,
        Recording,
        Schedule,
        User
    }
}
