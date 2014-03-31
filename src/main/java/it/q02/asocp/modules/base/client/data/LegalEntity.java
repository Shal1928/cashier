package it.q02.asocp.modules.base.client.data;

import com.google.gwt.user.client.rpc.IsSerializable;

/**
 * User: aleksander at  31.03.14, 18:58
 * Класс, описывает Юридеческое Лицо.
 */
public class LegalEntity implements IsSerializable {

    private String color;
    private String name="";
    private boolean editable = true;

    public String getColor() {
        return color;
    }

    public void setColor(String color) {
        this.color = color;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public boolean isEditable() {
        return editable;
    }

    public void setEditable(boolean editable) {
        this.editable = editable;
    }
}
