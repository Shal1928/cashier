using System;
using ASofCP.Cashier.Models.Contracts;

namespace ASofCP.Cashier.Stores.API
{
    // ReSharper disable InconsistentNaming
    public interface BaseAPI

    {
        /**
     * Возвращает список атракционов
     * @return
     */
        AttractionGroupInfo[] getGroups();

        /**
         * Возвращает список аттракционов в группу
         * @param group
         * @return
         */
        AttractionInfo[] getAttractionsFromGroup(AttractionGroupInfo group);

        /**
         * Возвращает список атракционов название которых совпадает с маской
         * @param mask
         * @return
         */
        //Attraction[] findAttractionBy(String mask);

        /**
         * Активирует бабаниу на кассе
         * @param series серия билет
         * @param nextTicket номер билета
         * @param color цвет билета
         * @return
         */
        RollInfo activateTicketRoll(String series, long nextTicket, RollColor color);

        /**
         * Деактивирует рулон билетов
         * @param series серия
         * @param nextTicket номер билета
         * @param color цвет
         * @return истина если успешно false если нет
         */
        bool deactivateTicketRoll(String series, long nextTicket, RollColor color);

        /**
         * Возвращает список цветов
         * @return
         */
        RollColor[] getColors();
    }
    // ReSharper restore InconsistentNaming
}
