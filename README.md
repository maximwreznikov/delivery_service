Simple delivery service on .NET Core.

Function service component: 
HTTP API. 
Methods:
GetAvailableDeliveries() - return deliveries, wich have status Available;
TakeDelivery(int userId, int deliveryId) - attach delivery for user (change status to Taken);
In case если доставка не найдена, return 404 с соответствующим сообщением в ответе;
In case если статус отличается от Available, return 422 с соответствующим сообщением в ответе;
Requirements:
Application must work "out of the box", i.e. it should not require installation of any additional software (only standart .NET Framework + Nuget packeges);
For storage data must be 2 variants: SQLite and PostgreSQL.
HTTP API have to developed with REST principe;
If unexpected error occurred when calling any method that does not return any stacktrace - return status 500 with message InternalServerError;
Field for delivery object: Id, Status, Title, UserId, CreationTime, ModificationTime;
PS:
For simlify task not necessary make user managment (create user, login/logout в систему, check user existance e.t.c.). 
We believe that every positive integer userId is valid;

Функциональные компоненты сервиса: 
HTTP API. Методы:
GetAvailableDeliveries() - возвращает доставки, доступные для взятия (в статусе Available);
TakeDelivery(int userId, int deliveryId) - закрепляет за пользователем доставку (перевод в статус Taken);
В случае если доставка не найдена, вернуть 404 с соответствующим сообщением в ответе;
В случае если статус отличается от Available, вернуть 422 с соответствующим сообщением в ответе;
Scheduler. Таски(not realized yet):
CreateDeliveries - создаёт новые доставки;
На каждое срабатывание таски создаётся от N до M (значения хранятся в конфиге) доставок;
Таска срабатывает по расписанию, заданному интервалом в конфиге в секундах 
(например, при интервале от 10 до 20 секунд - таска в первый раз может сработать через 13 секунд после запуска приложения, во второй через 17 и т.д.);
Созданные доставки находятся в статусе Available;
У созданных доставок проставляется ExpirationTime (время жизни доставки в секундах хранится в конфиге);
У созданных доставок проставлять поле Title случайным значением;
ExpireDeliveries - находит доставки, ExpirationTime которых находится в прошлом, а статус всё ещё Available и проставляет им статус Expired;
Требования:
Приложение должно работать "из коробки", т.е. не должно требовать установки никакого дополнительного софта (только стандартные библиотеки .NET Framework + Nuget пакеты, исключение - IIS);
Для хранения данных должно быть реализовано два варианта: sqlite и PostgreSQL. То, какой из них должен быть использован, задаётся в конфиге.
HTTP API должно быть разработано в соответствии с REST;
Если произошла непредвиденная ошибка при вызове какого-либо метода, не должно возвращаться никакого стэктрэйса - возвращаем 500-ю ошибку с сообщением InternalServerError;
Поля объекта доставки: Id, Status, Title, UserId, CreationTime, ModificationTime;
Примечания:
Для упрощения задачи не нужно реализовывать логику юзер менеджмента (создания юзеров, входа/выхода в систему, проверки существования пользователя и т.п.). 
Считаем, что любой положительный целочисленный userId валиден и существует;