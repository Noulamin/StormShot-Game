﻿/*using FunGames.Sdk.Notifs.Helpers;
            //_instance.StartCoroutine(_RequestAuthorization());
            var settings = Resources.Load<FunGamesSettings>("FunGamesSettings");

            if (settings.usePushNotifications)
            {
                Debug.Log("Initialize Notification");
#if UNITY_IOS
                FunGamesNotifs._instance.AskRequest();
#elif UNITY_ANDROID
            FunGamesNotifs.SetChannel("channel_id", "Default Channel", "Generic notifications");
#else
            Debug.LogWarning("You are not on a mobile device, it's going to do nothing");
#endif




#if UNITY_ANDROID
        /// <summary>
        /// all notifications must be assigned to a notification channel
        /// </summary>
        /// <param name="a_ChannelId">Set Id of the channel you want to create</param>
        /// <param name="a_Name">Set the name of the channel you want to create</param>
        /// <param name="a_Description">Set the descrition of the channel you want to create</param>
        /// <param name="a_Importance">Set importance of the channel you want to create default value <value>Importance.Default</param></param>


        #region TimeInternalNotifsAndroid
        /// <summary>
        /// Function to create schedule notification on Andorid, it's create <see cref="AndroidNotification"/> and then call <seealso cref="CreateTimeIntervalNotifs"/>
        /// </summary>
        /// <param name="a_Title">Set the title of the notification</param>
        /// <param name="a_Text">Set the title of the notification</param>
        /// <param name="a_ChannelId">Set the title of the notification</param>
        /// <param name="a_hours">Set the title of the notification</param>
        /// <param name="a_minutes">Set the title of the notification</param>
        /// <param name="a_seconds">Set the title of the notification</param>

        /// <summary>
        /// Use to get the user use to come on the app
        /// </summary>
        /// <returns>null if the user doesn't come from a notification</returns>
        {
            return AndroidNotificationCenter.GetLastNotificationIntent(); ;
        }

        /// <summary>
        /// Function that send the notification
        /// </summary>
        /// <param name="a_notification">Var that contains all the data for the notification</param>
        /// <param name="a_ChannelId">This for the id of the channel where the notification is send</param>
        /// You can use this function to remove all schedule notification
        /// </summary>
        #endregion
#elif UNITY_IOS
        /// Need to be call before creating a notification, but we call if it's not call before when creating a notification
        /// </summary>
        /// Use to get the user use to come on the app
        /// </summary>
        /// <returns>null if the user doesn't come from a notification</returns>
        {
            return iOSNotificationCenter.GetLastRespondedNotification();
        }


        #region TimeInternalNotifsIOS
        /// <summary>
        /// Function to create schedule notification on iOS, it's create <see cref="iOSNotificationTimeIntervalTrigger"/> and <see cref="iOSNotification"/> and then call CreateTimeIntervalNotifs <seealso cref="CreateTimeIntervalNotifs"/>
        /// </summary>
        /// <param name="hours">Number of hours the notification to be launched</param>
        /// <param name="minutes">Number of minutes the notification to be launched</param>
        /// <param name="seconds">Number of seconds the notification to be launched</param>
        /// <param name="a_Identifier">You have to set the id of the notification if you want to delete later</param>
        /// <param name="a_Title">The title of the notification</param>
        /// <param name="a_Body">The body of the notification</param>
        /// <param name="a_Subtitle">The subtitile of the notification</param>
        /// <param name="a_CategoryIdentifier">The category identifier of the notification, default value <value>Category_1</value></param>
        /// <param name="a_ThreadIdentifier">The thread identifier of the notification, default value <value>thread1</param>

        /// <summary>
        /// Function that create a schedule notification it also request authorization if it's not requested before
        /// </summary>
        /// <param name="notification">You will need to create a <see cref="iOSNotification"/> var to store all the value for the schedule notification</param>
        /// <returns><see cref="IEnumerator"/> need to be call in a <see cref="StartCoroutine"/></returns>