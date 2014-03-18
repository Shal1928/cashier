package it.q02.asocp.modules.base.client.ui;

import com.google.gwt.user.client.ui.IsWidget;

/**
 * User: aleksander at  18.03.14, 18:20
 */
public interface HasFilters {

    public static interface FilterCollection{

    }

    public IsWidget getFilterPanel();
    public void filterBy(FilterCollection collection);


}
