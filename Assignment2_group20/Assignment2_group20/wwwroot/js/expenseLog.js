"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/logExpenseHub").build();

connection.on("logExpense", function (expense) {
    
    var txt = expense;
    var li = document.createElement("li");
    li.textContent = txt;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});
