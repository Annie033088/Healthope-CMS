
export default function (mock) {
    mock.onPost("/api/Order/AddOrder").reply(() => {
        const orderIdDto = {
            OrderId: 1
        }
        return [200, { ErrorCode: 1, ApiDataObject: orderIdDto }];
    })
}