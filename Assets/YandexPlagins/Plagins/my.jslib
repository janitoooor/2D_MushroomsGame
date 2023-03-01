mergeInto(LibraryManager.library, {

    AuthExtern: function () {
        ysdk.getPlayer().then(_player => {
            player = _player;
        }).catch(err => {
            // Ошибка при инициализации объекта Player.
        });

        if (player.getMode() === 'lite') {
            ysdk.auth.openAuthDialog();
        }
        else {
            myGameInstance.SendMessage('AuthBonus', 'AuthBonusGems');
            ysdk.getLeaderboards()
                .then(_lb => lb = _lb);
        }
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

    AddCoinsExtern: function () {
        ysdk.adv.showRewardedVideo({
            callbacks: {
                onOpen: () => {
                    console.log('Video ad open.');
                },
                onRewarded: () => {
                    console.log('Rewarded!');
                    myGameInstance.SendMessage('ShowAdv', 'AddCoinsAdv');
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

    BuyItemGems200Extern: function () {

        payments.purchase({ id: 'gems200' }).then(purchase => {
            myGameInstance.SendMessage('InApp', 'GetGemsAfterBuying200');
            addGems(200).then(() => payments.consumePurchase(purchase.purchaseToken));
        });

        function addGems(value) {
            return player.incrementStats({ gems: value });
        }
    },

    BuyItemGems600Extern: function () {

        payments.purchase({ id: 'gems600' }).then(purchase => {
            myGameInstance.SendMessage('InApp', 'GetGemsAfterBuying600');
            addGems(600).then(() => payments.consumePurchase(purchase.purchaseToken));
        });

        function addGems(value) {
            return player.incrementStats({ gems: value });
        }
    },

    BuyItemGems2000Extern: function () {

        payments.purchase({ id: 'gems2000' }).then(purchase => {
            myGameInstance.SendMessage('InApp', 'GetGemsAfterBuying2000');
            addGems(2000).then(() => payments.consumePurchase(purchase.purchaseToken));
        });

        function addGems(value) {
            return player.incrementStats({ gems: value });
        }
    },

    CheckPaymentsExtern: function () {
        payments.getPurchases().then(purchases => purchases.forEach(consumePurchase));

        function consumePurchase(purchase) {
            if (purchase.productID === 'gems200') {
                myGameInstance.SendMessage('InApp', 'GetGemsAfterBuying200');
                player.incrementStats({ gems: 200 }).then(() => {
                    payments.consumePurchase(purchase.purchaseToken)
                });
            }
            if (purchase.productID === 'gems600') {
                myGameInstance.SendMessage('InApp', 'GetGemsAfterBuying600');
                player.incrementStats({ gems: 600 }).then(() => {
                    payments.consumePurchase(purchase.purchaseToken)
                });
            }
            if (purchase.productID === 'gems2000') {
                myGameInstance.SendMessage('InApp', 'GetGemsAfterBuying2000');
                player.incrementStats({ gems: 2000 }).then(() => {
                    payments.consumePurchase(purchase.purchaseToken)
                });
            }
        }
    },

});