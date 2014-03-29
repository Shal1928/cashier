package it.q02.asocp.modules.base.client.data;

import com.google.gwt.user.client.rpc.IsSerializable;

/**
 *
 */
public class CashVoucherItem implements IsSerializable {

    private String title;
    private double price;
    private int count = 1;


    public CashVoucherItem(){
        //
    }

    public CashVoucherItem(String title, double price) {
        this.title = title;
        this.price = price;
    }

    public CashVoucherItem(String title, double price, int count) {
        this.title = title;
        this.price = price;
        this.count = count;
    }


    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public double getTotal() {
        return getCount() * getPrice();
    }

    public int getCount() {
        return count;
    }

    public void setCount(int count) {
        if(count == 0) count = 1;
        this.count = count;
    }

    public double getPrice() {
        return price;
    }

    public void setPrice(double price) {
        this.price = price;
    }


    @Override
    public int hashCode() {
        return title.hashCode();
    }

    @Override
    public boolean equals(Object o) {
        if(o == null || !(o instanceof CashVoucherItem)) return false;

        CashVoucherItem that = (CashVoucherItem)o;
        return this.getTitle() != null && this.getTitle().equals(that.getTitle())
               && this.getPrice() == that.getPrice();
    }
}
