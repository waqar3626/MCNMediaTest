
//#region Initialize API
window.fbAsyncInit = function () {
    FB.init({
        appId: '220664392765773',
        autoLogAppEvents: true,
        status: true,
        cookie: true,                     // Enable cookies to allow the server to access the session.
        xfbml: true,                     // Parse social plugins on this webpage.
        version: 'v9.0'          // Use this Graph API version for this call.
    });


    FB.getLoginStatus(function (response) {   // Called after the JS SDK has been initialized.
        statusChangeCallback(response);        // Returns the login status.
    });


};
//#endregion

//#region Login & Login Status
function statusChangeCallback(response) {  // Called with the results from FB.getLoginStatus().
    console.log('statusChangeCallback');
    console.log(response);                   // The current login status of the person.
    if (response.status === 'connected') {   // Logged into your webpage and Facebook.
        $('#btnStatusfb').removeClass('btn-dark');
        $('#btnStatusfb').addClass('btn-success');
        // $('#btnRevokefb').show();
        //$('#btnConnectfb').css('display', 'none');
        $('#btnStatusfb').html('Online');
        $('.fb-login-button').hide();
        sessionStorage.setItem('uid', response.authResponse.userID);
        sessionStorage.setItem('uac', response.authResponse.accessToken);
        saveUserInfo(response.authResponse.accessToken, response.authResponse.userID, response.authResponse.expiresIn);
        //add user login details with user access token

    } else {                                 // Not logged into your webpage or we are unable to tell.
        $('#btnStatusfb').addClass('btn-dark');
        $('#btnStatusfb').removeClass('btn-success');
        // $('#btnRevokefb').hide();
        //  $('#btnConnectfb').css('display', 'normal');
        $('.fb-login-button').show();
        $('#btnStatusfb').html('Offline');
        // $('#startStreaming').css('disabled', 'disabled');
    }

    getPagesList();
    getCamInfo();
    getUserInfo();
    $('#stopStreaming').css('display', 'none');

    $('#streamsettings').show();
}
function checkLoginState() {               // Called when a person is finished with the Login Button.
    FB.getLoginStatus(function (response) {   // See the onlogin handler
        statusChangeCallback(response);
        if (response.authResponse != null) {
            $('#fbUserToken').val(response.authResponse.accessToken);
        }
    });
}
//#endregion

function Authenticate_User() {
    FB.api(
        '/oauth/authorize',
        'POST',
        { "client_id": "1517160011742266", "redirect_uri": "https://localhost:56182/Home/Index", "scope": "manage_pages" },
        function (response) {
            var myJSON = JSON.stringify(response);
            // alert(myJSON);
        }
    );
}

//#region Access Tokens
// Get Access Token
function GetAccessToken() {
    FB.api(
        '/oauth/access_token',
        'GET',
        {
            "": null, "redirect_uri": "https://localhost:56182/Home/Index", "client_secret": "3b5c89a1b47ab33e588b09e196a", "client_id": "1517160011742266", "code": ""
        },
        function (response) {
            var myJSON = JSON.stringify(response);
            // alert("Access_Token" + myJSON);
        }
    );
}

//Get Long Live Access Token
function GetLongLiveAccessToken() {
    FB.api(
        '/oauth/access_token',
        'GET',
        { "grant_type": "fb_exchange_token", "client_id": "1517160011742266", "client_secret": "3b56b47ab33e588b09e196a", "fb_exchange_token": "" + uac + "" },
        function (response) {
            var myJSON = JSON.stringify(response);
            // alert(myJSON);
            // Insert your code here
        }
    );
}


//Get Page Access Token
function GetPageAccessToken() {
    let uac = sessionStorage.getItem('uac');
    FB.api(
        '/10640226465',  //page_id
        'GET',
        { "fields": "access_token", "access_token": "" + uac + "" },
        function (response) {
            // Insert your code here
        }
    );
}
//#endregion

//#region Get User Pages
function getPagesList() {
    let uac = sessionStorage.getItem('uac');
    let uid = sessionStorage.getItem('uid');
    //  alert(uac);
    FB.api(
       // '/' + uid + '/accounts',
        '/me/accounts',
        'GET',
        { "fields": "name,access_token", "access_token": "" + uac + "" },
        function (response) {
            clearPageList();
            $.each(response.data, function (i, response) {
                var div_data = "<option value=" + response.id + " data-acctok=" + response.access_token + " >" + response.name + "</option>";
                //alert(div_data);
                $(div_data).appendTo('#facebook_page');
            });
        }

    );
}
//#endregion

//#region Streaming
function Request_LiveVedioObj() {
    
    var id = '';
    let uid = sessionStorage.getItem('uid');
    let uac = sessionStorage.getItem('uac');
    var accToken = '';
    if (uid != null && uac != null)
    {
        if ($('#camera_list').val() == 0) {

            swal({
                icon: "error",
                title: "Error",
                text: "Please select your camera first",

            });
            return;
        }
        if ($('#facebook_page').val() == "me") {
            id = "me";
            accToken = uac;
        }
        else {
            id = $('#facebook_page').val();
            accToken = $('#facebook_page option:selected').attr('data-acctok');
        }



        FB.api(
            '/' + id + '/live_videos',
            'POST',
            {
                "status": "LIVE_NOW", "access_token": "" + accToken + ""
            },


            function (response) {
              
                var jsons = JSON.stringify(response);
                // alert(jsons);
                var cameraId = $('#camera_list option:selected').attr('data-clientname') + '_' + $('#camera_list').val();
                var streamkey = response.stream_url.substring(response.stream_url.indexOf("rtmp/") + 5);
     
                var cameraInfo = {
                    camera_id: cameraId,
                    client_name: '',
                    type: $('#camera_list option:selected').attr('data-camtype'),
                    stream_url: "rtmp://live-api-s.facebook.com:80/rtmp/",
                    secure_stream_url: "rtmps://live-api-s.facebook.com:443/rtmp/",
                    stream_key: streamkey, //"108000807847714?s_bl=1&s_psm=1&s_sc=108000824514379&s_sw=0&s_vt=api-s&a=Abx34FLUBqF0nyj8"
                }
                //PosttoLiveStream(cameraInfo);
                StartLiveStreaming(cameraInfo);
            }
        );
    }
    else
    {
        swal({
            icon: "error",
            title: "Error",
            text: "Please login to facebook",

        });
       
    }
}


function StartLiveStreaming(cameraInfo) {
    var formData = new FormData();
    formData.append("jsonData", JSON.stringify(cameraInfo));
    formData.append("CamId", $('#camera_list').val());
    $.ajax({
        type: "POST",
        url: "/Client/LiveStreamToFacebook/",
        data: formData,
        processData: false,  // tell jQuery not to process the data
        contentType: false,  // tell jQuery not to set contentType
        success: function (result) {
            //alert(result);
            if (result) {
                swal({
                    icon: "success",
                    title: "Stream posted",
                    text: "Stream posted to facebook",

                });
               
                $('#stopStreaming').show();
                $('#startStreaming').hide();
            }
            else {
                swal({
                    icon: "error",
                    title: "Error",
                    text: "There is a problem posting to facebook.Please check your stream is started on camera",

                });
               
            }
        },
        error: function (errormessage) {
            swal({
                icon: "error",
                title: "Error",
                text: "Error when posting to facebook",

            });
          
            $('#stopStreaming').hide();
            $('#startStreaming').show();
        }
    });
}

function PosttoLiveStream(cameraInfo) {
    //chrome.exe --user-data-dir="C://Chrome dev session" --disable-web-security
    $.ajax({
        url: 'https://cors-anywhere.herokuapp.com/http://52.51.59.126:8182/api/v1/start-fb-live',
        type: 'post',
        crossDomain: true,
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(cameraInfo),
        beforeSend: function (request) {
            request.setRequestHeader("Access-Control-Allow-Origin", "*");
        },
        success: function (data) {
            if (data.status == "STREAM_NOT_FOUND") {
                swal({
                    icon: "info",
                    title: "start Stream",
                    text: "Please start your stream on camera",

                });
                
            }
            else {
                swal({
                    icon: "success",
                    title: "Stream Posted",
                    text: "Stream posted to facebook",

                });
              
                $('#stopStreaming').show();
                $('#startStreaming').hide();
            }
        },
        error: function (data) {
            swal({
                icon: "error",
                title: "Error",
                text: "Error when posting to facebook",

            });
           
            $('#stopStreaming').hide();
            $('#startStreaming').show();
        }

    });
}

function StopLiveStream() {
    var cameraInfo = {
        camera_id: $('#camera_list').val(),
        client_name: $('#camera_list option:selected').attr('data-clientname'),
        type: $('#camera_list option:selected').attr('data-camtype')
    }

    StopLiveStreaming(cameraInfo);
    //$.ajax({
    //    url: 'http://52.51.59.126:8182/api/v1/stop-fb-live',
    //    type: 'post',
    //    dataType: 'json',
    //    contentType: 'application/json',
    //    data: JSON.stringify(cameraInfo),

    //    success: function (data) {
    //        alert("Stream stopped on facebook");
    //        $('#startStreaming').css('display', 'normal');
    //        $('#stopStreaming').css('display', 'none');
    //    },
    //    error: function (data) {
    //        alert("Error when stopping stream");
    //        $('#startStreaming').css('display', 'none');
    //        $('#stopStreaming').css('display', 'normal');
    //    }

    //});
}

function StopLiveStreaming(cameraInfo) {
    var formData = new FormData();
    formData.append("jsonData", JSON.stringify(cameraInfo));
    $.ajax({
        type: "POST",
        url: "/Client/StopLiveStreamToFacebook/",
        data: formData,
        processData: false,  // tell jQuery not to process the data
        contentType: false,  // tell jQuery not to set contentType
        success: function (result) {
           // alert(result);
            swal({
                icon: "success",
                title: "Stream stopped",
                text: "Stream stopped on facebook",

            });
            
            $('#startStreaming').show();
            $('#stopStreaming').hide();
        },
        error: function (result) {
            swal({
                icon: "error",
                title: "Error",
                text: "Error when stopping stream",

            });
            
            $('#stopStreaming').show();
            $('#startStreaming').hide();
        }
    });
}
//#endregion




//#region Streaming Preferences
function changeOptions() {
    //getPagesList();
    getUserInfo();
    $("#streamsettings").toggle();

    if ($("#changeSettings").html() == "Change") {
        $("#changeSettings").html("Close");
    }
    else { $("#changeSettings").html("Change"); }
}

function savesettings() {
    if ($('#camera_list').val() == 0) {

        swal({
            icon: "error",
            title: "Error",
            text: "Please select your camera first",

        });
        return;
    }
    var formData = new FormData();
    formData.append("cameraId", $('#camera_list').val());
    formData.append("pageId", $('#facebook_page').val());
    formData.append("pageName", $('#facebook_page  option:selected').text());
    formData.append("pageAccessToken", $('#facebook_page option:selected').attr('data-acctok'));
    formData.append("description", $('#streamDescription').val());
    $.ajax({
        type: "POST",
        url: "/Client/SaveSettings/",

        data: formData,
        processData: false,  // tell jQuery not to process the data
        contentType: false,  // tell jQuery not to set contentType


        success: function (result) {
            swal({
                icon: "success",
                title: "Saved",
                text: "Settings have been updated",

            });
         

        },
        error: function (errormessage) {
            swal({
                icon: "error",
                title: "Error Occured",
                text: errormessage.responseText,

            });
        }
    });
}
//function getstreamparams() {
//    let uid = sessionStorage.getItem('uid');
//    let uac = sessionStorage.getItem('uac');
//    //if(uid != null && uac != null)
//    //{
//        var formData = new FormData();
//        formData.append("cameraId", $('#camera_list').val());
//        formData.append("pageId", $('#facebook_page').val());
//        formData.append("pageName", $('#facebook_page  option:selected').text());
//        formData.append("pageAccessToken", $('#facebook_page option:selected').attr('data-acctok'));
//        formData.append("description", $('#streamDescription').val());
//        formData.append("userId", uid);
//        formData.append("userAccessToken", uac);

//        $.ajax({
//            type: "POST",
//            url: "/Client/GetStreamParams/",

//            data: formData,
//            processData: false,  // tell jQuery not to process the data
//            contentType: false,  // tell jQuery not to set contentType


//            success: function (result) {
//                //alert("Settings have been updated");

//            },
//            error: function (errormessage) {
//                alert(errormessage.responseText);
//            }
//        });
//    //}
//    //else {
//    //    alert("Please login to facebook");
//    //}
//}
function saveUserInfo(accessToken, userId, expiresIn) {

    var formData = new FormData();
    // formData.append("userId", userId);
    formData.append("accessToken", accessToken);
    formData.append("tokenExpiry", expiresIn);
    formData.append("liveStatus", 1);


    $.ajax({
        type: "POST",
        url: "/Client/SaveUserInfo/",

        data: formData,
        processData: false,  // tell jQuery not to process the data
        contentType: false,  // tell jQuery not to set contentType


        success: function (result) {


        },
        error: function (errormessage) {
            swal({
                icon: "error",
                title: "Error Occured",
                text: errormessage.responseText,

            });
        }
    });
}

function getUserInfo() {

    $.ajax({
        type: "POST",
        url: "/Client/GetFbDetailsByUser/",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var pageid = result.page_Id;
            var camId = result.camera_Id;
            var msg = result.message;
            $('#facebook_page  option[value="' + pageid + '"]').prop("selected", true);
            $('#camera_list  option[value="' + camId + '"]').prop("selected", true);

            $('#streamDescription').val(msg);
        },
        error: function (errormessage) {
            swal({
                icon: "error",
                title: "Error Occured",
                text: errormessage.responseText,
                
            });
          
        }
    });
}

function getCamInfo() {
    $.ajax({
        url: "/Client/GetCamerasInfo/",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (response) {
            clearCamList();
            $.each(response, function (key, item) {
                var s = '';
                var clientname = '';
                if (item.streamingProtocol == "rtmp") {
                    var str = item.cameraUrl;
                    clientname = str.substring(0, str.indexOf("_"));
                    s = '<option value="' + item.cameraId + '" data-camtype="rtmp" data-clientname="' + clientname + '">' + item.cameraName + '</option>'
                }
                else {

                    clientname = item.churchUniqueIdentifier;
                    s = '<option value="' + item.cameraId + '" data-camtype="camera" data-clientname="' + clientname + '">' + item.cameraName + '</option>'
                }

                $(s).appendTo('#camera_list');
            });
        },
        error: function (errormessage) {
            // alert(errormessage.responseText);
        }
    });
}
//#endregion

//#region Clear Lists
function clearCamList() {
    $('#camera_list')
        .find('option')
        .remove()
        .end()
        .append('<option id="me" data-camtype="--Select--" data-clientname="--Select--" value="0">--Select--</option >');
}
function clearPageList() {
    $('#facebook_page')
        .find('option')
        .remove()
        .end()
        .append('<option value="me" data-acctok="Your Timeline" >My Timeline</option >');

}
//#endregion