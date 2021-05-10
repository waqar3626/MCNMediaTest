function ProfileSchedule(ChurchId) {
    
    $.ajax({
        url: "/Website/ProfileDataByJson/" + ChurchId,
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            NextScheduleEvent(result.nextSchedule);
            ProfileNotices(result.notice);
            $('.ScheduleEventList0').html(ScheduleListDay(result.scheduleListDay0));
            $('.ScheduleEventList1').html(ScheduleListDay(result.scheduleListDay1));
            $('.ScheduleEventList2').html(ScheduleListDay(result.scheduleListDay2));
            $('.ScheduleEventList3').html(ScheduleListDay(result.scheduleListDay3));
            $('.ScheduleEventList4').html(ScheduleListDay(result.scheduleListDay4));
            $('.ScheduleEventList5').html(ScheduleListDay(result.scheduleListDay5));
            $('.ScheduleEventList6').html(ScheduleListDay(result.scheduleListDay6));
            ProfileAnnouncement(result.announcement);
            ProfileNewsLetter(result.newsletter);
            ProfileChurchDonation(result.churchDonation);

        },
        error: function (errormessage) {
            // alert(errormessage.responseText);
        }
   });
}


function NextScheduleEvent(nextScheduleEvent) {

    var html='';
    if (nextScheduleEvent != null) {
        if (nextScheduleEvent.isRepeated == true && nextScheduleEvent.record == true) {
            html += '<b>' + nextScheduleEvent.eventName + ' <span class="text-pink">(</span>Weekly <span class="text-pink" >)</span > <i class="fa fa-video-camera text-danger"></i><br /></b > <br />';
        }
        else if (nextScheduleEvent.isRepeated == true && nextScheduleEvent.record == false) {
            html += '<b>' + nextScheduleEvent.eventName + ' <span class="text-pink">(</span>Weekly <span class="text-pink" >)</span ></b></br>';
        }
        else if (nextScheduleEvent.isRepeated == false && nextScheduleEvent.record == true) {
            html += '<b>' + nextScheduleEvent.eventName + ' <i class="fa fa-video-camera text-danger"></i></b></br>';
        }
        else {
            html += '<b>' + nextScheduleEvent.eventName+'</b>';
        }
        var eventTime = new Date(nextScheduleEvent.eventTime);
        
        var Time = eventTime.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true });
        
        html += '<p>' + nextScheduleEvent.eventDay + '  ' + Time + '</p>';

        


    } else {
        html += ' <p>No Events Scheduled</p>';
    }

    $('#nextScheduleEvent').html(html);
}

function formatAMPM(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'pm' : 'am';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return strTime;
}

function ScheduleListDay(scheduleListDay) {

    var html = '';
    if (scheduleListDay != null) {

        if (scheduleListDay.length > 0) {

            $.each(scheduleListDay, function (key, item) {
                html = '';
                html += '<tr class="text-white font-12 border-bottom" style="background-color: #2d5a89 !important;">';
                html += '<td scope="row" class="font-weight-normal text-purple">';
                if (item.isRepeated == true && item.record == true) {
                    html += '<span>' + item.eventName + ' <span class="text-pink">(</span>Weekly <span class="text-pink" >)</span > </span > ';
                }
                else if (item.isRepeated == true && item.record == false) {
                    html += '<span>' + item.eventName + ' <span class="text-pink">(</span>Weekly <span class="text-pink" >)</span ></span>';
                }
                else if (item.isRepeated == false && item.record == true) {
                    html += '<span>' + item.eventName + '</span>';
                }
                else {
                    html += '<span>' + item.eventName + '</span>';
                }
                html += '</td>';
                var eventTime = new Date(item.eventTime);

                var Time = eventTime.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true });
                html +='<td scope = "row" class="font-weight-normal" >'+Time+' </td >'
                if (item.record == true) {
                    html += '<td class="font-weight-normal"><i class="fa fa-video text-red"></i></td>';
                }
                else {
                    html += '<td class="font-weight-normal"></td>'
                    html += '</tr>';
                }
            });
        }
        else {
            html = '<tr class="text-white font-12 border-bottom" style="background-color: #2d5a89 !important;"><td scope = "row" class="font-weight-normal text-purple" >There is currently nothing scheduled for today.</td ></tr >';
        }
    }
    else {
        html = '<tr class="text-white font-12 border-bottom" style="background-color: #2d5a89 !important;"><td scope = "row" class="font-weight-normal text-purple" >There is currently nothing scheduled for today.</td ></tr >';
    }

    return html;
}

function ProfileNotices(profileNotice) {
    
    var html = '';
    if (profileNotice != null) {
        if (profileNotice.churchNoticeId != 0) {
        html += '<div class="text-praaa mt-2 mb-2"><div class="border bg-white rounded-lg">';
        html += '<div class="card-header-custom text-white font-20 pl-3 w-100 rounded-top" style = "background-color: #1f4a79;" >Notices</div >';
        html += '<div class="news-wrap mb-2 p-2"><div class="font-25 bg-white font-weight-bold ProximaNova" >';
        html += profileNotice.noticeTitle + '</div>';
        var noticeName = document.createElement('div');
        noticeName.innerHTML = profileNotice.noticeName;
        html += '<p class="font-16 mb-2">' + noticeName.textContent || noticeName.innerText || "" + '</p>';
        html += '</div></div ></div >'
        } else {
            $('#ProfileNotices').hide();
            html = '';
        }
    } else {
        $('#ProfileNotices').hide();
        html = '';
    }
    
    $('#ProfileNotices').html(html);

}

function ProfileAnnouncement(profileAnnouncement) {

    var html = '';
    if (profileAnnouncement != null) {
        if (profileAnnouncement.churchAnnouncementId != 0) {
            html += '<div class="text-praaa mt-2 mb-2 text-left"><div class="border bg-white rounded-lg">';
            html += '<div class="card-header-custom text-white pl-3 font-20  w-100 rounded-top" style = "background-color: #1f4a79;" >Announcement</div >';
            html += '<div class="news-wrap mb-2 p-2"><div class="font-25 bg-white font-weight-bold ProximaNova" >';
            html += profileAnnouncement.announcementTitle + '</div>';
            var announcementName = document.createElement('div');
            announcementName.innerHTML = profileAnnouncement.announcementText;
            html += '<p class="font-16 mb-2 ">' + announcementName.textContent || announcementName.innerText || "" + '</p>';
            html += '</div></div ></div >'
        } else {
            $('#ProfileAnnouncement').hide();
            html = '';
        }
    } else {
        $('#ProfileAnnouncement').hide();
        html = '';
    }

    $('#ProfileAnnouncement').html(html);

}

function ProfileChurchDonation(churchDonation) {
    var html = '';
    if (churchDonation != null) {

        if (churchDonation.donationId != 0) {

            if (churchDonation.showOnWebsite == true) {
                html += '  <div class="form-group row news">';
                html += '<div class="col-sm-12"><div class="max-width-100 max-height-100 " >';
                html += '<img src="'+churchDonation.imageUrl+'" class="max-height-100 w-100  rounded-top" style="border-top-left-radius:30px;border-top-right-radius:30px; " alt="Alternate Text" />';
                html += '<a href="' + churchDonation.webSiteUrl + '" target="_blank" class="btn btn-info rounded-bottom w-100" style="border-bottom-left-radius:30px;border-bottom-right-radius:30px; ">Donate Now</a>';
                html += '</div></div ></div >';
            }
            else {
                $('#ProfileChurchDonation').hide();
                html = '';
            }
        }
        else {
            $('#ProfileChurchDonation').hide();
            html = '';
        }

    }
    else {
        $('#ProfileChurchDonation').hide();
        html = '';
    }

    $('#ProfileChurchDonation').html(html);
}

function ProfileNewsLetter(newsletter) {

    var html = '';
   
   
        
    if (newsletter != null) {

        if (newsletter.churchNewsLetterId != 0) {

            html += ' <div class="card-header-custom text-white font-20 text-center w-100 rounded-top   " style="background-color: #1f4a79;">';
            html += 'NEWSLETTER</div>';
            html += '<div class="news-wrap mb-3 p-4">';
            html += ' <h4 class="news-heading font-16 font-weight-bold">' + newsletter.newsLetterTitle + '</h4>';
            html += '<p class="font-weight-light mb-2 font-12">';
            html += '<a href="' + newsletter.newsLetterUrl + '" target="_blank">Click Here</a>';
            html += '</p><p class="font-14 mb-0 " ></p >';
            html += '<p class="font-14 mb-0 ">' + newsletter.createdAt + '</p></div>';
        }
        else { 
            $('#ProfileNewsLetter').hide();
            html = '';
        }
    }
    else {
        $('#ProfileNewsLetter').hide();
        html = '';
    }


    $('#ProfileNewsLetter').html(html);


}