﻿$(function () {
	 var menuItem = $('#left-sidebar-menu-account');
    menuItem.addClass('active');
    var subMenuItem = menuItem.find('#left-sidebar-menu-account-userprofile');
    subMenuItem.addClass('active');

    $("#BirthDate").datepicker({ format: "yyyy-mm-dd" });
});