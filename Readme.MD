﻿# Выгрузка отчета по ЖНВЛС для Росздравнадзора из еФарма 2 A.K.A F3 Tail, производства [Спарго технологии](http://spargo.ru)

###### Автор Руденко Евгений
###### Вы можете связаться со мной по телефону +79098040505
###### или через мой сайт [APTEKA27.COM](http://APTEKA27.COM)

## Что нужно знать:
### Минусы
 - Программа не проверяет ваши данные на правильность, превышение наценки
 - Программа не использует какие любо привязки, даже если они есть в вашей БД, лишь ищет соответствие по штрих-коду товара
 - Если в базе данных указан неверный штрих-код, или вы при загрузке электронной накладной указали неверный завод/наименование, то отчет будет неверным и возможно превышение цены
### Плюсы
 - Отчет формируется за пол минуты
 - Не нужна подготовка справочника товара  для работы. Достаточно наличия штрих-кода товара в БД.
 
 ##Как установить и настроить
 
 - Скачать проект, собрать его в Visual Studio
 - Если нет учетной записи, то зарегистрировать аптеку на сайте [Росздравнадзора](http://mols.roszdravnadzor.ru/?type=logon&redirect_params=) 
 - В настройках на сайте указать список поставщиков (их нужно найти по ИНН
 - Подключиться к SQL и накатать скрипты на БД
 - Contractors.sql
 - Заполнить коды в таблице id_contractor - код в БД Росздравнадзора, id_contractor_efarma - код поставщика из еФарма 2
 - Накатить скрипт roszdravnadzor_tbl.sql 
 - Накатить скрипт goods.sql - загрузится номенклатура с сайта Росздравнадзора (начало 2018 года. если нужна новее, то нужно скачать файл c сайта и подготовить данные для загрузки)
 - Накатить скрипт formatdatetime.sql - он нужен, чтобы сформировать даты по шаблону.
 - В каталоге, куда вы собрали программу найти Roszdravnadzor.exe.config.  В нем заполняем строку подключения к БД
`<setting name="farmaConnectionString" serializeAs="String">
 <value>Data Source=localhost\sqlexpress;Initial Catalog=eplus_work;User ID=sa;Password=111;</value>
</setting>
`  
 - Запустить программу, сформировать отчет для Росздравнадзора, загрузить файл через импорт на сайте
 - Удалить все строки, где есть превышения
 - Забыть об отчете еще на месяц
