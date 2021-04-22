(function ($) {
    'use strict';
    $.contextMenu({
        selector: '#context-menu-simple',
        callback: function (key, options) { },
        items: {
            "edit": {
                name: "Edit",
                icon: "edit"
            },
            "cut": {
                name: "Cut",
                icon: "cut"
            },
            copy: {
                name: "Copy",
                icon: "copy"
            },
            "paste": {
                name: "Paste",
                icon: "paste"
            },
            "delete": {
                name: "Delete",
                icon: "delete"
            },
            "sep1": "---------",
            "quit": {
                name: "Quit",
                icon: function () {
                    return 'context-menu-icon context-menu-icon-quit';
                }
            }
        }
    });

    $.contextMenu({
        selector: '#context-menu-access',
        callback: function (key, options) {
            var m = "clicked: " + key;
            window.console && console.log(m) || alert(m);
        },
        items: {
            "edit": {
                name: "Edit",
                icon: "edit",
                accesskey: "e"
            },
            "cut": {
                name: "Cut",
                icon: "cut",
                accesskey: "c"
            },
            // first unused character is taken (here: o)
            "copy": {
                name: "Copy",
                icon: "copy",
                accesskey: "c o p y"
            },
            // words are truncated to their first letter (here: p)
            "paste": {
                name: "Paste",
                icon: "paste",
                accesskey: "cool paste"
            },
            "delete": {
                name: "Delete",
                icon: "delete"
            },
            "sep1": "---------",
            "quit": {
                name: "Quit",
                icon: function ($element, key, item) {
                    return 'context-menu-icon context-menu-icon-quit';
                }
            }
        }
    });
    $.contextMenu({
        selector: '#context-menu-open',
        callback: function (key, options) {
            var m = "clicked: " + key;
            window.console && console.log(m) || alert(m);
        },
        items: {
            "edit": {
                name: "Closing on Click",
                icon: "edit",
                callback: function () {
                    return true;
                }
            },
            "cut": {
                name: "Open after Click",
                icon: "cut",
                callback: function () {
                    return false;
                }
            }
        }
    });

    $.contextMenu({
        selector: '#context-menu-multi',
        callback: function (key, options) {
            var m = "clicked: " + key;
            window.console && console.log(m) || alert(m);
        },
        items: {
            "edit": {
                "name": "Edit",
                "icon": "edit"
            },
            "cut": {
                "name": "Cut",
                "icon": "cut"
            },
            "sep1": "---------",
            "quit": {
                "name": "Quit",
                "icon": "quit"
            },
            "sep2": "---------",
            "fold1": {
                "name": "Sub group",
                "items": {
                    "fold1-key1": {
                        "name": "Foo bar"
                    },
                    "fold2": {
                        "name": "Sub group 2",
                        "items": {
                            "fold2-key1": {
                                "name": "alpha"
                            },
                            "fold2-key2": {
                                "name": "bravo"
                            },
                            "fold2-key3": {
                                "name": "charlie"
                            }
                        }
                    },
                    "fold1-key3": {
                        "name": "delta"
                    }
                }
            },
            "fold1a": {
                "name": "Other group",
                "items": {
                    "fold1a-key1": {
                        "name": "echo"
                    },
                    "fold1a-key2": {
                        "name": "foxtrot"
                    },
                    "fold1a-key3": {
                        "name": "golf"
                    }
                }
            }
        }
    });

    $.contextMenu({
        selector: '#context-menu-hover',
        trigger: 'hover',
        delay: 500,
        callback: function (key, options) {
            var m = "clicked: " + key;
            window.console && console.log(m) || alert(m);
        },
        items: {
            "edit": {
                name: "Edit",
                icon: "edit"
            },
            "cut": {
                name: "Cut",
                icon: "cut"
            },
            "copy": {
                name: "Copy",
                icon: "copy"
            },
            "paste": {
                name: "Paste",
                icon: "paste"
            },
            "delete": {
                name: "Delete",
                icon: "delete"
            },
            "sep1": "---------",
            "quit": {
                name: "Quit",
                icon: function ($element, key, item) {
                    return 'context-menu-icon context-menu-icon-quit';
                }
            }
        }
    });

    $.contextMenu({
        selector: '#context-menu-hover-autohide',
        trigger: 'hover',
        delay: 500,
        autoHide: true,
        callback: function (key, options) {
            var m = "clicked: " + key;
            window.console && console.log(m) || alert(m);
        },
        items: {
            "edit": {
                name: "Edit",

                icon: "edit"
            },
            "cut": {
                name: "Cut",
                icon: "cut"
            },
            "copy": {
                name: "Copy",
                icon: "copy"
            },
            "paste": {
                name: "Paste",
                icon: "paste"
            },
            "delete": {
                name: "Delete",
                icon: "delete"
            },
            "sep1": "---------",
            "quit": {
                name: "Quit",
                icon: function ($element, key, item) {
                    return 'context-menu-icon context-menu-icon-quit';
                }
            }
        }
    });

    $.contextMenu({
        selector: '#context-menu-hover-autohideNew2',
        trigger: 'hover',
        delay: 500,
        autoHide: true,
        callback: function (key, options) {
            var m = "clicked: " + key;
            var cameraId = options.$trigger[0].getAttribute('cameraId');
            if (key == "edit") {
                Open(cameraId);
            }
            else if (key == "delete") {
                DeleteCamera(cameraId);
            }
       
           
            
        },
        items: {
            "edit": {
                name: "Edit",
                id: "CameraID2",
                icon: "edit"
            },

            "delete": {
                name: "Delete",
                icon: "delete"
            }
        }
    });

    $.contextMenu({
        selector: '#context-menu-hover-autohidePictures',
        trigger: 'hover',
        delay: 500,
        autoHide: true,
        callback: function (key, options) {
            var m = "clicked: " + key;
            var mediachurchid = options.$trigger[0].getAttribute('mediachurchid');
          
            if (key == "edit") {
                EditPicture(mediachurchid);
            }
            else if (key == "delete") {
                DeletePicture(mediachurchid);
            }

        },
        items: {
            "edit": {
                name: "Edit",
                id: "CameraID2",
                icon: "edit"
            },

            "delete": {
                name: "Delete",
                icon: "delete"
            }
        }
    });



    $.contextMenu({
        selector: '#context-menu-hover-autohideVideos',
        trigger: 'hover',
        delay: 500,
        autoHide: true,
        callback: function (key, options) {
            var m = "clicked: " + key;
            var mediachurchid = options.$trigger[0].getAttribute('mediachurchid');
            if (key == "edit") {
                EditVideo(mediachurchid);
            }
            else if (key == "delete") {
                DeleteRecording(mediachurchid);
            }
            else if (key == "play") {

                window.location.href = "../Client/ClientVideoPlayer/" + mediachurchid;
            }
        },
        items: {
            "edit": {
                name: "Edit",
                id: "CameraID2",
                icon: "edit"
            },

            "delete": {
                name: "Delete",
                icon: "delete"
            },

            "play": {
                name: "Play",
                icon: "play"
            }
        }
    });


    $.contextMenu({
        selector: '#context-menu-hover-autohideSlideShow',
        trigger: 'hover',
        delay: 500,
        autoHide: true,
        callback: function (key, options) {
            var m = "clicked: " + key;
            var mediachurchid = options.$trigger[0].getAttribute('mediachurchid');
            if (key == "edit") {
                EditSlideShow(mediachurchid);
            }
            else if (key == "delete") {
                DeleteSlideShow(mediachurchid);
            }


        },
        items: {
            "edit": {
                name: "Edit",
                id: "CameraID2",
                icon: "edit"
            },

            "delete": {
                name: "Delete",
                icon: "delete"
            
            }

        }
    });

    $.contextMenu({
        selector: '#context-menu-hover-autohideNewsLetter',
        trigger: 'hover',
        delay: 500,
        autoHide: true,
        callback: function (key, options) {
            var m = "clicked: " + key;
            var churchNewsletterid = options.$trigger[0].getAttribute('churchNewsLetterId');
            if (key == "delete") {
                DeleteNewsLetter(churchNewsletterid);
            }
           


        },
        items: {
           
            "delete": {
                name: "Delete",
                icon: "delete"

            }

        }
    });


    $.contextMenu({
        selector: '#context-menu-hover-autohideRecording',
        trigger: 'hover',
        delay: 500,
        autoHide: true,
        callback: function (key, options) {
            var m = "clicked: " + key;
            var recordingId = options.$trigger[0].getAttribute('recordingId');
            var recordingSrc = options.$trigger[0].getAttribute('recordingSrc');
            if (key == "edit") {
                
                EditRecording(recordingId);
            }
            else if (key == "delete") {
                DeleteRecordingClick(recordingId);
            }
            else if (key == "download") {
              
                onDownloadClick(recordingSrc);
       
            }


        },
        items: {
            "edit": {
                name: "Edit",
                id: "CameraID2",
                icon: "edit"
            },

            "delete": {
                name: "Delete",
                icon: "delete"
            },
            "download": {
                name: "Download",
                icon: "download"
            }
        }
    });

})(jQuery);