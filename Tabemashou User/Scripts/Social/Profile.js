$(function () {
    IsFollowing();
});

function IsFollowing() {
    $.ajax({
        type: "POST",
        url: "/Social/IsFollowing/",
        data: { 'id': idUser },
        success: function (following) {
            if (following === 'True') {
                $("#followButton").hide();
                $("#unfollowButton").show();
            } else if (following === 'False') {
                $("#followButton").show();
                $("#unfollowButton").hide();
            }
        },
        error: function (xhr) {
            alert('Something went wrong.');
        }
    });
}

function Follow(id) {
    $.ajax({
        type: "POST",
        url: "/Social/Follow/",
        data: { 'id': id },
        success: function (result) {
            IsFollowing();
        },
        error: function (xhr) {
            alert('Something went wrong.');
        }
        
    });
}

function Unfollow(id) {
    $.ajax({
        type: "POST",
        url: "/Social/Unfollow/",
        data: { 'id': id }, 
        success: function (result) {
            IsFollowing();
        },
        error: function (xhr) {
            alert('Something went wrong.');
        }
    });
}
