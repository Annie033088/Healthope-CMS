
export default function (mock) {
    mock.onPost("/api/Order/AddOrderWithMembershipPlan").reply(() => {
        const orderIdDto = {
            OrderId: 1
        }
        return [200, { ErrorCode: 1, ApiDataObject: orderIdDto }];
    })

    mock.onPost("/api/Order/AddOrderWithPersonalTrainingPackage").reply(() => {
        const orderIdDto = {
            OrderId: 1
        }
        return [200, { ErrorCode: 1, ApiDataObject: orderIdDto }];
    })

    mock.onPost("/api/Order/AddOrder").reply(() => {
        const orderIdDto = {
            OrderId: 1
        }
        return [200, { ErrorCode: 1, ApiDataObject: orderIdDto }];
    })

    mock.onPost("/api/Order/PayByCard").reply(() => {
        return new Promise(resolve => {
            setTimeout(() => {
                const isSuccess = Math.random() > 0.5;
                resolve([200, { ErrorCode: isSuccess ? 10 : 1, ApiDataObject:{QrCodeString:"kpqwjej12312opkwqeopqkw12"} }]);
            }, 3000);
        });
    });

    mock.onPost("/api/Order/PayByCash").reply(() => {
        return [200, { ErrorCode: 1, ApiDataObject:{QrCodeString:"kpqwjej12312opkwqeopqkw12"} }];
    })
}