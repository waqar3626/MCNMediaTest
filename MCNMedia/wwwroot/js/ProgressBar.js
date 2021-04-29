var intervalId;

function startUpdatingProgressIndicator(progress, fileSize) {
    $("#progress").show();
    if (fileSize <= 50) {
        intervalId = setInterval(
            function () {
                if (progress < 100) {
                    progress++;

                }
                else {
                    progress = progress;
                }

                // We use the POST requests here to avoid caching problems (we could use the GET requests and disable the cache instead)
                $("#progress").css({ width: progress + "%" });
                $("#label").html(progress + "%");


            },
            300
        );
    }
    else if (fileSize > 50 && fileSize <= 100) {
        intervalId = setInterval(
            function () {
                if (progress < 100) {
                    progress++;

                }
                else {
                    progress = progress;
                }

                // We use the POST requests here to avoid caching problems (we could use the GET requests and disable the cache instead)
                $("#progress").css({ width: progress + "%" });
                $("#label").html(progress + "%");


            },
            700
        );
    }

    else if (fileSize > 100 && fileSize <= 150) {
        intervalId = setInterval(
            function () {
                if (progress < 100) {
                    progress++;

                }
                else {
                    progress = progress;
                }

                // We use the POST requests here to avoid caching problems (we could use the GET requests and disable the cache instead)
                $("#progress").css({ width: progress + "%" });
                $("#label").html(progress + "%");


            },
            1000
        );
    }
    else if (fileSize > 150 && fileSize <= 200) {
        intervalId = setInterval(
            function () {
                if (progress < 100) {
                    progress++;

                }
                else {
                    progress = progress;
                }

                // We use the POST requests here to avoid caching problems (we could use the GET requests and disable the cache instead)
                $("#progress").css({ width: progress + "%" });
                $("#label").html(progress + "%");


            },
            1500
        );
    }

    else if (fileSize > 200 && fileSize <= 300) {
        intervalId = setInterval(
            function () {
                if (progress < 100) {
                    progress++;

                }
                else {
                    progress = progress;
                }

                // We use the POST requests here to avoid caching problems (we could use the GET requests and disable the cache instead)
                $("#progress").css({ width: progress + "%" });
                $("#label").html(progress + "%");


            },
            2000
        );
    }
    else if (fileSize > 400 && fileSize <= 500) {
        intervalId = setInterval(
            function () {
                if (progress < 100) {
                    progress++;

                }
                else {
                    progress = progress;
                }

                // We use the POST requests here to avoid caching problems (we could use the GET requests and disable the cache instead)
                $("#progress").css({ width: progress + "%" });
                $("#label").html(progress + "%");


            },
            3000
        );
    }
    else if (fileSize > 500 && fileSize <= 750) {
        intervalId = setInterval(
            function () {
                if (progress < 100) {
                    progress++;

                }
                else {
                    progress = progress;
                }

                // We use the POST requests here to avoid caching problems (we could use the GET requests and disable the cache instead)
                $("#progress").css({ width: progress + "%" });
                $("#label").html(progress + "%");


            },
          4000
        );
    }
    else if (fileSize > 750 && fileSize <= 1000) {
        intervalId = setInterval(
            function () {
                if (progress < 100) {
                    progress++;

                }
                else {
                    progress = progress;
                }

                // We use the POST requests here to avoid caching problems (we could use the GET requests and disable the cache instead)
                $("#progress").css({ width: progress + "%" });
                $("#label").html(progress + "%");


            },
            5000
        );
    }
    else if (fileSize > 1000 && fileSize <= 1500) {
        intervalId = setInterval(
            function () {
                if (progress < 100) {
                    progress++;

                }
                else {
                    progress = progress;
                }

                // We use the POST requests here to avoid caching problems (we could use the GET requests and disable the cache instead)
                $("#progress").css({ width: progress + "%" });
                $("#label").html(progress + "%");


            },
            7500
        );
    }
    else  {
        intervalId = setInterval(
            function () {
                if (progress < 100) {
                    progress++;

                }
                else {
                    progress = progress;
                }

                // We use the POST requests here to avoid caching problems (we could use the GET requests and disable the cache instead)
                $("#progress").css({ width: progress + "%" });
                $("#label").html(progress + "%");


            },
            10000
        );
    }
}

function stopUpdatingProgressIndicator() {
    clearInterval(intervalId);


}
function sleep(miliseconds) {
    var currentTime = new Date().getTime();

    while (currentTime + miliseconds >= new Date().getTime()) {
    }
}