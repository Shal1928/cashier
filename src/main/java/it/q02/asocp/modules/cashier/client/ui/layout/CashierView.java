package it.q02.asocp.modules.cashier.client.ui.layout;

import com.google.gwt.core.client.GWT;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.uibinder.client.UiField;
import com.google.gwt.user.cellview.client.Header;
import com.google.gwt.user.cellview.client.TextColumn;
import com.google.gwt.user.cellview.client.TextHeader;
import com.google.gwt.user.client.ui.IsWidget;
import com.google.gwt.user.client.ui.Widget;
import it.q02.asocp.modules.base.client.data.CashVoucher;
import it.q02.asocp.modules.base.client.data.CashVoucherItem;
import it.q02.asocp.modules.cashier.client.ui.resources.Resources;
import org.gwtbootstrap3.client.ui.Column;
import org.gwtbootstrap3.client.ui.DataGrid;
import org.gwtbootstrap3.client.ui.FluidContainer;

/**
 *
 */
public class CashierView implements IsWidget {
    //region UiBinder base
    interface CashierViewUiBinder extends UiBinder<FluidContainer, CashierView> {
    }

    private static CashierViewUiBinder ourUiBinder = GWT.create(CashierViewUiBinder.class);
    //endregion


    //region UiFields
    private FluidContainer rootElement;

    @UiField(provided = true)
    Resources res;

    @UiField
    DataGrid dataGrid;
    //endregion


    public CashierView() {
        res = GWT.create(Resources.class);
        res.layoutStyle().ensureInjected();
        rootElement = ourUiBinder.createAndBindUi(this);

        TextColumn titleCol = new TextColumn<CashVoucherItem>() {
            @Override
            public String getValue(CashVoucherItem cashVoucherItem) {
                return cashVoucherItem.getTitle();
            }
        };
        TextColumn priceCol = new TextColumn<CashVoucherItem>() {
            @Override
            public String getValue(CashVoucherItem cashVoucherItem) {
                return String.valueOf(cashVoucherItem.getPrice());
            }
        };
        TextColumn countCol = new TextColumn<CashVoucherItem>() {
            @Override
            public String getValue(CashVoucherItem cashVoucherItem) {
                return String.valueOf(cashVoucherItem.getCount());
            }
        };
        TextColumn totalCol = new TextColumn<CashVoucherItem>() {
            @Override
            public String getValue(CashVoucherItem cashVoucherItem) {
                return String.valueOf(cashVoucherItem.getTotal());
            }
        };

        dataGrid.addColumn(titleCol, "Наименование");
        dataGrid.addColumn(priceCol, "Цена");
        dataGrid.addColumn(countCol, "Количество");
        dataGrid.addColumn(totalCol, "Итого");

        dataGrid.setColumnWidth(0, "60%");
        dataGrid.setColumnWidth(1, "15%");
        dataGrid.setColumnWidth(2, "10%");
        dataGrid.setColumnWidth(3, "15%");

        dataGrid.setRowData(getTestData());
    }

    private CashVoucher getTestData(){
        CashVoucher cashVoucher = new CashVoucher();
        cashVoucher.add(new CashVoucherItem("Котята жаренные в масле", 145.92));
        cashVoucher.add(new CashVoucherItem("Фаршированный котенок", 110.56));
        cashVoucher.add(new CashVoucherItem("Живой котенок", 10.54));
        cashVoucher.add(new CashVoucherItem("Лапка котенка", 50.63));
        cashVoucher.add(new CashVoucherItem("Счастливая лапка котенка", 51.00));
        cashVoucher.add(new CashVoucherItem("Колесо", 80.45));
        cashVoucher.add(new CashVoucherItem("Колеса", 85.41));
        cashVoucher.add(new CashVoucherItem("Будка", 32.90));
        cashVoucher.add(new CashVoucherItem("Большое приключение", 15.12));
        cashVoucher.add(new CashVoucherItem("Вино из одуванчиков", 250.00));
        cashVoucher.add(new CashVoucherItem("Кепа", 84.31));
        cashVoucher.add(new CashVoucherItem("42", 42));

        return cashVoucher;
    }

    //region IsWidget
    @Override
    public Widget asWidget() {
        return rootElement;
    }
    //endregion
}