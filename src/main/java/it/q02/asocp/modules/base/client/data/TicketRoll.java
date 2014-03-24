package it.q02.asocp.modules.base.client.data;

import com.google.gwt.user.client.rpc.IsSerializable;

import java.util.Date;

/**
 * User: aleksander at  22.03.14, 18:57
 */
public class TicketRoll implements IsSerializable{

    public static enum State {

    }

    private long id;

    private String firstTicketNumber;

    private long ticketCount;

    private String color;

    private Date createDate;

    private String owner;

    private TicketRollActivationInfo activationInfo;

    public static class TicketRollActivationInfo implements IsSerializable{

        private long id;

        private String cashier;

        private String station;

        private Date activationDate;

        public long getId() {
            return id;
        }

        public void setId(long id) {
            this.id = id;
        }

        public String getCashier() {
            return cashier;
        }

        public void setCashier(String cashier) {
            this.cashier = cashier;
        }

        public String getStation() {
            return station;
        }

        public void setStation(String station) {
            this.station = station;
        }

        public Date getActivationDate() {
            return activationDate;
        }

        public void setActivationDate(Date activationDate) {
            this.activationDate = activationDate;
        }
    }

    public long getId() {
        return id;
    }

    public void setId(long id) {
        this.id = id;
    }

    public String getFirstTicketNumber() {
        return firstTicketNumber;
    }

    public void setFirstTicketNumber(String firstTicketNumber) {
        this.firstTicketNumber = firstTicketNumber;
    }

    public long getTicketCount() {
        return ticketCount;
    }

    public void setTicketCount(long ticketCount) {
        this.ticketCount = ticketCount;
    }

    public String getColor() {
        return color;
    }

    public void setColor(String color) {
        this.color = color;
    }

    public Date getCreateDate() {
        return createDate;
    }

    public void setCreateDate(Date createDate) {
        this.createDate = createDate;
    }

    public String getOwner() {
        return owner;
    }

    public void setOwner(String owner) {
        this.owner = owner;
    }

    public TicketRollActivationInfo getActivationInfo() {
        return activationInfo;
    }

    public void setActivationInfo(TicketRollActivationInfo activationInfo) {
        this.activationInfo = activationInfo;
    }

    public boolean isActived(){
        return getActivationInfo()!=null;
    }


}
