package it.q02.asocp.modules.administrator.client.activities.tickets.dialogs;

import com.google.gwt.cell.client.*;
import com.google.gwt.core.client.GWT;
import com.google.gwt.dom.client.DivElement;
import com.google.gwt.dom.client.Element;
import com.google.gwt.uibinder.client.UiBinder;
import com.google.gwt.uibinder.client.UiField;
import com.google.gwt.user.cellview.client.Column;
import com.google.gwt.user.cellview.client.TextColumn;
import com.google.gwt.view.client.ListDataProvider;
import it.q02.asocp.modules.base.client.data.LegalEntity;
import it.q02.asocp.modules.base.client.ui.components.cells.ColorSelectionCell;
import org.gwtbootstrap3.client.shared.event.ModalHideEvent;
import org.gwtbootstrap3.client.shared.event.ModalHideHandler;
import org.gwtbootstrap3.client.ui.CellTable;

import org.gwtbootstrap3.client.ui.Modal;

import java.util.Arrays;
import java.util.LinkedList;
import java.util.List;

/**
 * User: aleksander at  31.03.14, 18:49
 */
public class LegalEntityEditor {

    private final Modal dialog;

    @UiField(provided = true)
    protected CellTable<LegalEntity> legalEntitys;

    private List<LegalEntity> editedList = new LinkedList<>();

    public LegalEntityEditor() {
        final ListDataProvider<LegalEntity> entityListDataProvider = new ListDataProvider<>();
        legalEntitys = new CellTable(30);
        Column<LegalEntity, ?> column;
        legalEntitys.addColumn(column = new Column<LegalEntity, Boolean>(new CheckboxCell()) {
            @Override
            public Boolean getValue(LegalEntity legalEntity) {
                return editedList.contains(legalEntity);
            }
        },"");
        column.setFieldUpdater((FieldUpdater)new FieldUpdater<LegalEntity, Boolean>() {
            @Override
            public void update(int i, LegalEntity legalEntity, Boolean aBoolean) {
                if(editedList.contains(legalEntity)){
                    editedList.remove(legalEntity);
                }else{
                    editedList.add(legalEntity);
                }
                entityListDataProvider.refresh();
            }
        });

        legalEntitys.addColumn(column=new Column<LegalEntity, String>(new EditTextCell(){
            @Override
            protected void edit(Context context, Element parent, String value) {
                if(context.getKey() instanceof LegalEntity && ((LegalEntity) context.getKey()).isEditable()){
                    super.edit(context,parent,value);
                }
            }
        }) {
            @Override
            public String getValue(LegalEntity legalEntity) {
                return legalEntity.getName();
            }
        },"Название");

        column.setFieldUpdater((FieldUpdater)new FieldUpdater<LegalEntity, String>() {
            @Override
            public void update(int i, LegalEntity legalEntity, String newValue) {
                legalEntity.setName(newValue);
                entityListDataProvider.refresh();
            }
        });

        legalEntitys.setColumnWidth(1, "80%");
        legalEntitys.addColumn(column=new Column<LegalEntity, String>(new ColorSelectionCell(Arrays.asList("red","green","blue","yellow"))) {
            @Override
            public String getValue(LegalEntity legalEntity) {
                return legalEntity.getColor();
            }
        }, "Цвет");
        column.setFieldUpdater((FieldUpdater)new FieldUpdater<LegalEntity, String>() {
            @Override
            public void update(int i, LegalEntity legalEntity, String value) {
                legalEntity.setColor(value);
                entityListDataProvider.refresh();
            }
        });



        entityListDataProvider.setList(Arrays.asList(new LegalEntity(){
            @Override
            public boolean isEditable() {
                return false;
            }
        },new LegalEntity(),new LegalEntity(),new LegalEntity()));
        entityListDataProvider.addDataDisplay(legalEntitys);
        entityListDataProvider.refresh();

        dialog = ourUiBinder.createAndBindUi(this);
        dialog.addHideHandler(new ModalHideHandler() {
            @Override
            public void onHide(ModalHideEvent modalHideEvent) {

            }
        });
    }



    public void show(){
        dialog.show();
    }


    interface LegalEntityEditorUiBinder extends UiBinder<Modal, LegalEntityEditor> {
    }

    private static LegalEntityEditorUiBinder ourUiBinder = GWT.create(LegalEntityEditorUiBinder.class);


}