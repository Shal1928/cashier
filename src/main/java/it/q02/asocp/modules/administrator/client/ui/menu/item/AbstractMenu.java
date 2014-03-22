package it.q02.asocp.modules.administrator.client.ui.menu.item;

import com.google.gwt.user.client.Command;

/**
 * User: aleksander at  22.03.14, 17:57
 */
public abstract class AbstractMenu implements MenuItem,Command {

    private String name;

    private String id;

    protected AbstractMenu(String name, String id) {
        this.name = name;
        this.id = id;
    }

    public final String getName() {
        return name;
    }

    public final String getId() {
        return id;
    }


    public final Command getCommand() {
        return  this;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        AbstractMenu menuItem = (AbstractMenu) o;

        if (id != null ? !id.equals(menuItem.id) : menuItem.id != null) return false;

        return true;
    }

    @Override
    public int hashCode() {
        return id != null ? id.hashCode() : 0;
    }



}
