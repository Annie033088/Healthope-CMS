import MockAdapter from 'axios-mock-adapter'
import axios from 'axios'


export default function setupMock() {
    let mockAdapter = new MockAdapter(axios)

    mockAdapter.onPost("/api/Login/LoggedIn").reply(200, {
        ErrorCode: 1
    })

    mockAdapter.onPost("/api/Login/LoggedOut").reply(200, {
        ErrorCode: 1
    })

}