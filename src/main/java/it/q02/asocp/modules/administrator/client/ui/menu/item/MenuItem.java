package it.q02.asocp.modules.administrator.client.ui.menu.item;

import com.google.gwt.user.client.Command;
import org.gwtbootstrap3.client.ui.ListItem;
import org.gwtbootstrap3.client.ui.constants.IconType;

/**
 * User: aleksander at  22.03.14, 17:19
 */
public interface MenuItem {

    public String getId();
    public String getName();
    public Command getCommand();

}
