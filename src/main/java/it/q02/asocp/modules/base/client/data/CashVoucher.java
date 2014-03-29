package it.q02.asocp.modules.base.client.data;

import java.util.Collection;
import java.util.LinkedList;

/**
 *
 */
public class CashVoucher extends LinkedList<CashVoucherItem> {

    public CashVoucher(){
        //
    }

    @Override
    public boolean add(CashVoucherItem newItem) {
        if(!this.contains(newItem)) return super.add(newItem);

        for(CashVoucherItem item:this)
            if(item.equals(newItem)) {
                item.setCount(item.getCount() + newItem.getCount());
                return true;
            }

        return false;
    }

    @Override
    public boolean addAll(Collection<? extends CashVoucherItem> c) {
        for(CashVoucherItem item:c)
            if(!this.add(item)) return false;

        return true;
    }
}
