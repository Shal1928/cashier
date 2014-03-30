package it.q02.asocp.modules.base.client.data;

import com.google.gwt.user.client.rpc.IsSerializable;

import java.util.Date;

/**
 * User: aleksander at  29.03.14, 18:21
 */
public class Attraction implements IsSerializable {

    private String versionSeries;

    private long id;

    private String code;

    private String name;

    private String printName;

    private long price;

    private boolean isActive;

    private Date createDate;

    private String owner;

    private long minPrice;

    public long getMinPrice() {
        return minPrice;
    }

    public void setMinPrice(long minPrice) {
        this.minPrice = minPrice;
    }

    public String getVersionSeries() {
        return versionSeries;
    }

    public void setVersionSeries(String versionSeries) {
        this.versionSeries = versionSeries;
    }

    public long getId() {
        return id;
    }

    public void setId(long id) {
        this.id = id;
    }

    public String getCode() {
        return code;
    }

    public void setCode(String code) {
        this.code = code;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getPrintName() {
        return printName;
    }

    public void setPrintName(String printName) {
        this.printName = printName;
    }

    public long getPrice() {
        return price;
    }

    public void setPrice(long price) {
        this.price = price;
    }

    public boolean isActive() {
        return isActive;
    }

    public void setActive(boolean active) {
        isActive = active;
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

    public boolean hasSales() {
        return false;  //To change body of created methods use File | Settings | File Templates.
    }

    public void cloneFrom(Attraction attraction) {
        //todo: clone
    }
}
