using System;

// ReSharper disable CheckNamespace
namespace it.q02.asocp.api.data
// ReSharper restore CheckNamespace
{
    public class TicketInfo
    {
        //Идентификатор билета в базе.
        public long Id;

        //Штрих-код билета
        public String Barcode;

        //Логин кассира продавшего билет
        public String Cashier;

        //Название атракциона
        public String Name;

        //Цена по которой был продан билет
        public long Price;

        //Дата закрытия билета
        public DateTime CloseDate;

        //Тип оплаты
        public int PayType;

        //Название кассы на которой был пробит билет
        public String PosName;

        //Билет сторнирован
        public bool IsReversed;

        //Логин кассира сторнировавшего билет
        public String ReversedCashier;

        //true если в процессе выполнения запроса произошла ошибка
        public bool RequestError;

        //содержит описание ошибки.
        public String ErrorMessage;
    }
}
