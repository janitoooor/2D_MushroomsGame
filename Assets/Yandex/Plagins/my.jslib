mergeInto(LibraryManager.library, {

    Hello: function () {
        window.alert("Hello, world!");
        console.log("Hello, world!");
    },

    GetPlayerData: function () {
        myGameInstance.SendMessage('Yandex', 'SetName', player.getName());
        myGameInstance.SendMessage('Yandex', 'SetPhoto', player.getPhoto("medium"));
    },

    RateGame: function () {
        ysdk.feedback.canReview()
            .then(({ value, reason }) => {
                if (value) {
                    ysdk.feedback.requestReview()
                        .then(({ feedbackSent }) => {
                            console.log(feedbackSent);
                        })
                } else {
                    console.log(reason)
                }
            })
    },

    SaveExtern: function (date) {
        var dateString = UTF8ToString(date);
        var myobj = JSON.parse(dateString);
        player.setData(myobj, true);
    },

    LoadExtern: function () {
        player.getData().then(_date => {
            const myJSON = JSON.stringify(_date);
            myGameInstance.SendMessage('JsonSaveSystem', 'Load', myJSON);
        });
    },

    ShowAdv: function () {
        ysdk.adv.showFullscreenAdv({
            callbacks: {
                onClose: function (wasShown) {
                },
                onError: function (error) {
                }
            }
        })
    },


    AddCoinsExtern: function (value) {
        ysdk.adv.showRewardedVideo({
            callbacks: {
                onOpen: () => {
                    console.log('Video ad open.');
                },
                onRewarded: () => {
                    console.log('Rewarded!');
                    myGameInstance.SendMessage('Yandex', 'AddCoinsForAdv', value);
                },
                onClose: () => {
                    console.log('Video ad closed.');
                },
                onError: (e) => {
                    console.log('Error while open video ad:', e);
                }
            }
        })
    },

    SetToLeaderboard: function (value) {
        ysdk.getLeaderboards()
            .then(lb => {
                lb.setLeaderboardScore('MushroomSpores', value);
            });
    },

});