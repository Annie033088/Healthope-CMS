export default function (mock) {
    mock.onPost("/api/Coach/AddCoach").reply(() => {
        // for (let [key, value] of config.data.entries()) {
        //     console.log(key, value);
        // }
        return [200, { ErrorCode: 1 }]
    })
}