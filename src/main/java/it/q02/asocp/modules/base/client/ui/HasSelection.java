package it.q02.asocp.modules.base.client.ui;

import com.google.gwt.event.logical.shared.HasSelectionHandlers;
import com.google.gwt.event.logical.shared.SelectionEvent;

import java.util.List;

/**
 * User: aleksander at  18.03.14, 17:49
 */
public interface HasSelection<T> {

    public void addSelectionListener(HasSelectionHandlers<T> selectionHandlers);

    public List<T> getSelection();

    public void clearSelection();

    public boolean isMultiSelect();

    public void setSelected(List<T> object);

}
