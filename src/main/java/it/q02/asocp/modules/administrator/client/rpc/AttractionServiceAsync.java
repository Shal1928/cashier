package it.q02.asocp.modules.administrator.client.rpc;

import com.google.gwt.user.client.rpc.AsyncCallback;
import it.q02.asocp.modules.base.client.data.Attraction;

import java.util.List;

/**
 * User: aleksander at  29.03.14, 20:43
 */
public interface AttractionServiceAsync {

    void saveAndUpdate(Attraction attraction, AsyncCallback<Attraction> async);
    void find(AsyncCallback<List<Attraction>> async);

}

