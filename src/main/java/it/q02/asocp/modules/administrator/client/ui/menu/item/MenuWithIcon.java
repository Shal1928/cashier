package it.q02.asocp.modules.administrator.client.ui.menu.item;

import it.q02.asocp.modules.administrator.client.ui.menu.item.AbstractMenu;
import it.q02.asocp.modules.administrator.client.ui.menu.item.HasIcon;
import org.gwtbootstrap3.client.ui.constants.IconType;

/**
* User: aleksander at  23.03.14, 10:25
*/
public abstract class MenuWithIcon extends AbstractMenu implements HasIcon {
    private IconType icon;

    public MenuWithIcon(String displayName, String id, IconType icon) {
        super(displayName,id);
        this.icon = icon;
    }

    @Override
    public IconType getIcon() {
        return icon;
    }
}
