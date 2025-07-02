export default function (mock) {
    const orders = [
        {
            "OrderId": 15,
            "MemberId": 103,
            "MemberName": "QX",
            "MemberPhone": 987654321,
            "PlanId": 2,
            "PlanType": 2,
            "PlanName": "12 堂基本課",
            "OrderNumber": 250616586860000012,
            "State": 3,
            "Amount": 15000,
            "Method": 1,
            "Remark": "",
            "CreateTime": "2025-06-16T08:18:06.390",
            "UpdateTime": "2025-06-28T09:37:28.762"
        },
        {
            "OrderId": 5,
            "MemberId": 106,
            "MemberName": "ZK",
            "MemberPhone": 987654322,
            "PlanId": 2,
            "PlanType": 1,
            "PlanName": "3 個月會籍",
            "OrderNumber": 250616533060000012,
            "State": 2,
            "Amount": 2900,
            "Method": 1,
            "Remark": "",
            "CreateTime": "2025-06-16T06:48:27.913",
            "UpdateTime": "2025-06-16T06:48:35.003"
        },
        {
            "OrderId": 21,
            "MemberId": 111,
            "MemberName": "TG",
            "MemberPhone": 987654323,
            "PlanId": 2,
            "PlanType": 1,
            "PlanName": "3 個月會籍",
            "OrderNumber": 250619607730000012,
            "State": 5,
            "Amount": 2900,
            "Method": 2,
            "Remark": "",
            "CreateTime": "2025-06-19T08:52:53.236",
            "UpdateTime": "2025-06-28T10:04:46.780"
        },
        {
            "OrderId": 33,
            "MemberId": 108,
            "MemberName": "BL",
            "MemberPhone": 987654324,
            "PlanId": 2,
            "PlanType": 1,
            "PlanName": "3 個月會籍",
            "OrderNumber": 250624545020000012,
            "State": 3,
            "Amount": 2900,
            "Method": 1,
            "Remark": "",
            "CreateTime": "2025-06-24T07:08:22.996",
            "UpdateTime": "2025-06-28T07:42:30.148"
        },
        {
            "OrderId": 49,
            "MemberId": 112,
            "MemberName": "NS",
            "MemberPhone": 987654325,
            "PlanId": 2,
            "PlanType": 1,
            "PlanName": "3 個月會籍",
            "OrderNumber": 250630359800000012,
            "State": 5,
            "Amount": 2900,
            "Method": 2,
            "Remark": "",
            "CreateTime": "2025-06-30T01:59:41.010",
            "UpdateTime": "2025-06-30T02:32:01.134"
        },
        {
            "OrderId": 3,
            "MemberId": 107,
            "MemberName": "ME",
            "MemberPhone": 987654326,
            "PlanId": 2,
            "PlanType": 1,
            "PlanName": "3 個月會籍",
            "OrderNumber": 250616516160000012,
            "State": 1,
            "Amount": 2900,
            "Method": 1,
            "Remark": "",
            "CreateTime": "2025-06-16T06:20:18.245",
            "UpdateTime": "2025-06-16T06:20:18.245"
        },
        {
            "OrderId": 32,
            "MemberId": 110,
            "MemberName": "RD",
            "MemberPhone": 987654327,
            "PlanId": 3,
            "PlanType": 1,
            "PlanName": "半年會籍",
            "OrderNumber": 250623635640000012,
            "State": 3,
            "Amount": 6000,
            "Method": 2,
            "Remark": "",
            "CreateTime": "2025-06-23T09:39:24.031",
            "UpdateTime": "2025-06-26T05:08:17.625"
        },
        {
            "OrderId": 13,
            "MemberId": 104,
            "MemberName": "UV",
            "MemberPhone": 987654328,
            "PlanId": 2,
            "PlanType": 2,
            "PlanName": "12 堂基本課",
            "OrderNumber": 250616567700000012,
            "State": 2,
            "Amount": 15000,
            "Method": 1,
            "Remark": "",
            "CreateTime": "2025-06-16T07:46:10.668",
            "UpdateTime": "2025-06-24T09:08:36.987"
        },
        {
            "OrderId": 36,
            "MemberId": 105,
            "MemberName": "HE",
            "MemberPhone": 987654329,
            "PlanId": 1,
            "PlanType": 1,
            "PlanName": "一年會籍",
            "OrderNumber": 250624579140000012,
            "State": 4,
            "Amount": 10000,
            "Method": 1,
            "Remark": "",
            "CreateTime": "2025-06-24T08:05:14.967",
            "UpdateTime": "2025-06-30T09:48:18.424"
        },
        {
            "OrderId": 17,
            "MemberId": 109,
            "MemberName": "AZ",
            "MemberPhone": 997654321,
            "PlanId": 1,
            "PlanType": 3,
            "PlanName": "一次性票?",
            "OrderNumber": 250616592470000012,
            "State": 2,
            "Amount": 250,
            "Method": 1,
            "Remark": "",
            "CreateTime": "2025-06-16T08:27:27.399",
            "UpdateTime": "2025-06-24T09:15:23.148"
        },
        {
            "OrderId": 29,
            "MemberId": 101,
            "MemberName": "CJ",
            "MemberPhone": 977654321,
            "PlanId": 1,
            "PlanType": 3,
            "PlanName": "一次性票卷",
            "OrderNumber": 250623544830000012,
            "State": 2,
            "Amount": 250,
            "Method": 1,
            "Remark": "",
            "CreateTime": "2025-06-23T07:08:03.923",
            "UpdateTime": "2025-06-23T07:08:03.977"
        },
        {
            "OrderId": 23,
            "MemberId": 102,
            "MemberName": "WD",
            "MemberPhone": 967654321,
            "PlanId": 1,
            "PlanType": 1,
            "PlanName": "一年會籍",
            "OrderNumber": 250620357950000012,
            "State": 2,
            "Amount": 10000,
            "Method": 1,
            "Remark": "",
            "CreateTime": "2025-06-20T01:56:35.716",
            "UpdateTime": "2025-06-20T01:56:35.795"
        },
        {
            "OrderId": 40,
            "MemberId": 113,
            "MemberName": "YX",
            "MemberPhone": 957654321,
            "PlanId": 1,
            "PlanType": 1,
            "PlanName": "一年會籍",
            "OrderNumber": 250624642350000012,
            "State": 2,
            "Amount": 10000,
            "Method": 1,
            "Remark": "",
            "CreateTime": "2025-06-24T09:50:35.165",
            "UpdateTime": "2025-06-24T09:50:35.265"
        },
        {
            "OrderId": 50,
            "MemberId": 114,
            "MemberName": "MP",
            "MemberPhone": 947654321,
            "PlanId": 2,
            "PlanType": 1,
            "PlanName": "3 個月會籍",
            "OrderNumber": 250630382790000012,
            "State": 5,
            "Amount": 2900,
            "Method": 2,
            "Remark": "",
            "CreateTime": "2025-06-30T02:37:59.516",
            "UpdateTime": "2025-06-30T02:39:08.976"
        },
        {
            "OrderId": 1,
            "MemberId": 115,
            "MemberName": "DK",
            "MemberPhone": 937654321,
            "PlanId": 2,
            "PlanType": 1,
            "PlanName": "3 個月會籍",
            "OrderNumber": 250616507540000012,
            "State": 2,
            "Amount": 2900,
            "Method": 1,
            "Remark": "睡個覺不",
            "CreateTime": "2025-06-16T06:05:57.565",
            "UpdateTime": "2025-06-24T07:41:32.584"
        }
    ]

    const orderStates = [
        {
            "OrderStateId": 1,
            "OrderId": 1,
            "State": 2,
            "Remark": "付款完成",
            "CreateTime": "2025-06-20T10:00:12.000Z",
            "UpdateTime": "2025-06-20T10:02:15.000Z"
        },
        {
            "OrderStateId": 12,
            "OrderId": 1,
            "State": 1,
            "Remark": "待付款",
            "CreateTime": "2025-06-20T10:00:12.000Z",
            "UpdateTime": "2025-06-20T10:02:15.000Z"
        },
        {
            "OrderStateId": 2,
            "OrderId": 2,
            "State": 2,
            "Remark": "處理中",
            "CreateTime": "2025-06-19T15:20:33.000Z",
            "UpdateTime": "2025-06-19T15:30:00.000Z"
        },
        {
            "OrderStateId": 3,
            "OrderId": 3,
            "State": 6,
            "Remark": "處理中",
            "CreateTime": "2025-06-18T08:15:42.000Z",
            "UpdateTime": "2025-06-18T08:16:10.000Z"
        },
        {
            "OrderStateId": 4,
            "OrderId": 4,
            "State": 3,
            "Remark": "已出貨",
            "CreateTime": "2025-06-17T11:00:00.000Z",
            "UpdateTime": "2025-06-17T11:10:00.000Z"
        },
        {
            "OrderStateId": 5,
            "OrderId": 5,
            "State": 4,
            "Remark": "完成",
            "CreateTime": "2025-06-16T12:00:00.000Z",
            "UpdateTime": "2025-06-16T13:00:00.000Z"
        },
        {
            "OrderStateId": 6,
            "OrderId": 6,
            "State": 5,
            "Remark": "已取消",
            "CreateTime": "2025-06-15T10:00:00.000Z",
            "UpdateTime": "2025-06-15T10:05:00.000Z"
        },
        {
            "OrderStateId": 7,
            "OrderId": 7,
            "State": 2,
            "Remark": "補發票",
            "CreateTime": "2025-06-14T09:20:00.000Z",
            "UpdateTime": "2025-06-14T09:25:00.000Z"
        },
        {
            "OrderStateId": 8,
            "OrderId": 8,
            "State": 3,
            "Remark": "發票失敗",
            "CreateTime": "2025-06-13T14:30:00.000Z",
            "UpdateTime": "2025-06-13T14:35:00.000Z"
        },
        {
            "OrderStateId": 9,
            "OrderId": 9,
            "State": 1,
            "Remark": "付款完成",
            "CreateTime": "2025-06-12T16:45:00.000Z",
            "UpdateTime": "2025-06-12T16:50:00.000Z"
        }
    ]

    const electronicInvoices = [
        {
            "ElectronicInvoiceId": 33,
            "OrderId": 35,
            "InvoiceNumber": "AC00000030",
            "InvoiceTime": "1900-01-01T00:00:00.000",
            "RandomNumber": "5267",
            "Buyer": "00000000",
            "TotalAmount": 2900,
            "Type": 1,
            "Category": 1,
            "Status": 3,
            "CreateTime": "2025-06-24T08:00:05.123"
        },
        {
            "ElectronicInvoiceId": 38,
            "OrderId": 13,
            "InvoiceNumber": "AC00000035",
            "InvoiceTime": "1900-01-01T00:00:00.000",
            "RandomNumber": "4172",
            "Buyer": "00000000",
            "TotalAmount": 15000,
            "Type": 1,
            "Category": 1,
            "Status": 3,
            "CreateTime": "2025-06-24T09:08:36.987"
        },
        {
            "ElectronicInvoiceId": 41,
            "OrderId": 40,
            "InvoiceNumber": "AC00000038",
            "InvoiceTime": "1900-01-01T00:00:00.000",
            "RandomNumber": "7799",
            "Buyer": "00000000",
            "TotalAmount": 10000,
            "Type": 1,
            "Category": 1,
            "Status": 3,
            "CreateTime": "2025-06-24T09:50:35.265"
        },
        {
            "ElectronicInvoiceId": 47,
            "OrderId": 39,
            "InvoiceNumber": "AC00000044",
            "InvoiceTime": "2025-06-30T09:12:43.000",
            "RandomNumber": "6142",
            "Buyer": "00000000",
            "TotalAmount": 6000,
            "Type": 1,
            "Category": 1,
            "Status": 7,
            "CreateTime": "2025-06-30T09:12:41.777"
        },
        {
            "ElectronicInvoiceId": 11,
            "OrderId": 19,
            "InvoiceNumber": "AC00000010",
            "InvoiceTime": "2025-06-16T08:33:07.000",
            "RandomNumber": "8655",
            "Buyer": "00000000",
            "TotalAmount": 250,
            "Type": 1,
            "Category": 1,
            "Status": 2,
            "CreateTime": "2025-06-16T08:33:02.833"
        },
        {
            "ElectronicInvoiceId": 24,
            "OrderId": 6,
            "InvoiceNumber": "AC00000021",
            "InvoiceTime": "2025-06-20T06:58:28.000",
            "RandomNumber": "8980",
            "Buyer": "00000000",
            "TotalAmount": 10000,
            "Type": 1,
            "Category": 1,
            "Status": 2,
            "CreateTime": "2025-06-20T06:58:27.384"
        },
        {
            "ElectronicInvoiceId": 19,
            "OrderId": 3,
            "InvoiceNumber": "AC00000059",
            "InvoiceTime": "2025-06-20T06:58:28.000",
            "RandomNumber": "8985",
            "Buyer": "00000000",
            "TotalAmount": 8000,
            "Type": 1,
            "Category": 1,
            "Status": 2,
            "CreateTime": "2025-06-20T06:58:27.384"
        },
        {
            "ElectronicInvoiceId": 90,
            "OrderId": 3,
            "InvoiceNumber": "AC00000066",
            "InvoiceTime": "2025-06-25T02:58:28.000",
            "RandomNumber": "8310",
            "Buyer": "00000000",
            "TotalAmount": 1000,
            "Type": 1,
            "Category": 2,
            "Status": 3,
            "CreateTime": "2025-06-20T06:58:27.384"
        },
        {
            "ElectronicInvoiceId": 14,
            "OrderId": 20,
            "InvoiceNumber": "AC00000013",
            "InvoiceTime": "2025-06-19T08:35:21.000",
            "RandomNumber": "7815",
            "Buyer": "00000000",
            "TotalAmount": 250,
            "Type": 1,
            "Category": 1,
            "Status": 2,
            "CreateTime": "2025-06-19T08:35:19.439"
        },
        {
            "ElectronicInvoiceId": 52,
            "OrderId": 36,
            "InvoiceNumber": "AC00000049",
            "InvoiceTime": "1900-01-01T00:00:00.000",
            "RandomNumber": "9375",
            "Buyer": "00000000",
            "TotalAmount": 2000,
            "Type": 1,
            "Category": 2,
            "Status": 1,
            "CreateTime": "2025-06-30T09:48:18.424"
        },
        {
            "ElectronicInvoiceId": 6,
            "OrderId": 8,
            "InvoiceNumber": "AC00000005",
            "InvoiceTime": "1900-01-01T00:00:00.000",
            "RandomNumber": "1808",
            "Buyer": "00000000",
            "TotalAmount": 15000,
            "Type": 1,
            "Category": 1,
            "Status": 3,
            "CreateTime": "2025-06-16T07:10:48.956"
        },
        {
            "ElectronicInvoiceId": 25,
            "OrderId": 27,
            "InvoiceNumber": "AC00000022",
            "InvoiceTime": "2025-06-23T05:47:04.000",
            "RandomNumber": "7105",
            "Buyer": "00000000",
            "TotalAmount": 15000,
            "Type": 1,
            "Category": 1,
            "Status": 2,
            "CreateTime": "2025-06-23T05:47:03.224"
        },
        {
            "ElectronicInvoiceId": 5,
            "OrderId": 7,
            "InvoiceNumber": "AC00000004",
            "InvoiceTime": "2025-06-16T07:02:07.000",
            "RandomNumber": "8376",
            "Buyer": "00000000",
            "TotalAmount": 2900,
            "Type": 1,
            "Category": 1,
            "Status": 2,
            "CreateTime": "2025-06-16T07:02:05.876"
        },
        {
            "ElectronicInvoiceId": 29,
            "OrderId": 31,
            "InvoiceNumber": "AC00000026",
            "InvoiceTime": "2025-06-23T09:08:11.000",
            "RandomNumber": "6011",
            "Buyer": "00000000",
            "TotalAmount": 6000,
            "Type": 1,
            "Category": 1,
            "Status": 5,
            "CreateTime": "2025-06-23T09:08:09.476"
        },
        {
            "ElectronicInvoiceId": 18,
            "OrderId": 22,
            "InvoiceNumber": "AC00000015",
            "InvoiceTime": "2025-06-20T01:50:05.000",
            "RandomNumber": "8878",
            "Buyer": "00000000",
            "TotalAmount": 15000,
            "Type": 1,
            "Category": 1,
            "Status": 2,
            "CreateTime": "2025-06-20T01:50:05.009"
        },
        {
            "ElectronicInvoiceId": 2,
            "OrderId": 4,
            "InvoiceNumber": "AC00000001",
            "InvoiceTime": "1900-01-01T00:00:00.000",
            "RandomNumber": "5469",
            "Buyer": "00000000",
            "TotalAmount": 10000,
            "Type": 1,
            "Category": 1,
            "Status": 3,
            "CreateTime": "2025-06-16T06:23:52.274"
        },
        {
            "ElectronicInvoiceId": 45,
            "OrderId": 49,
            "InvoiceNumber": "AC00000042",
            "InvoiceTime": "2025-06-30T02:17:25.000",
            "RandomNumber": "4273",
            "Buyer": "00000000",
            "TotalAmount": 2900,
            "Type": 1,
            "Category": 1,
            "Status": 7,
            "CreateTime": "2025-06-30T02:17:05.210"
        }
    ]

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
                resolve([200, { ErrorCode: isSuccess ? 10 : 1, ApiDataObject: { QrCodeString: "kpqwjej12312opkwqeopqkw12" } }]);
            }, 3000);
        });
    });

    mock.onPost("/api/Order/PayByCash").reply(() => {
        return [200, { ErrorCode: 1, ApiDataObject: { QrCodeString: "kpqwjej12312opkwqeopqkw12" } }];
    })

    mock.onPost("/api/Order/GetOrder").reply((config) => {
        let {
            State,
            Method,
            SortOrder,
            SortOption,
            RecordPerPage,
            Page
        } = JSON.parse(config.data);

        // 1️⃣ 篩選
        let filtered = orders.filter(item => {
            const matchStatus = State === null || item.State === Number(State);
            const matchMethod = Method === null || item.Method === Number(Method);
            return matchStatus && matchMethod;
        });

        let field;
        // 2️⃣ 排序
        if (SortOption === "amount") {
            field = "Amount";
        } else if (SortOption === "state") {
            field = "State";
        }
        else {
            field = "OrderId"
        }

        filtered.sort((a, b) => {
            let aVal = a[field];
            let bVal = b[field];

            if (aVal < bVal) return SortOrder === 'descending' ? 1 : -1;
            if (aVal > bVal) return SortOrder === 'descending' ? -1 : 1;
            return 0;
        });

        // 3️⃣ 分頁
        const start = (Page - 1) * RecordPerPage;
        const paged = filtered.slice(start, start + RecordPerPage);

        const ApiDataObject = {
            OrderList: paged,
            TotalPage: Math.ceil(
                filtered.length / RecordPerPage
            )
        }

        return [200, { ErrorCode: 1, ApiDataObject }]
    })

    mock.onPost("/api/Order/GetOrderDetailById").reply((config) => {
        let orderIdDto = JSON.parse(config.data);
        let orderTarget = orders.find(order => order.OrderId === Number(orderIdDto.OrderId));
        let orderStateTargets = orderStates.filter(orderState => orderState.OrderId === Number(orderIdDto.OrderId));
        let electronicInvoiceTargets = electronicInvoices.filter(electronicInvoice => electronicInvoice.OrderId === Number(orderIdDto.OrderId));

        const ApiDataObject = { Order: orderTarget, OrderStateList: orderStateTargets, InvoiceList: electronicInvoiceTargets }

        if (orderTarget) {
            return [200, { ErrorCode: 1, ApiDataObject }]
        } else {
            return [200, { ErrorCode: 13 }]
        }
    })

    mock.onPost("/api/Order/EditOrderStateRemark").reply((config) => {
        let editOrderStateRemarkDto = JSON.parse(config.data);
        let orderStateTarget = orderStates.find(orderState => orderState.OrderStateId === Number(editOrderStateRemarkDto.OrderStateId));

        if (orderStateTarget) {
            orderStateTarget.Remark = editOrderStateRemarkDto.Remark
        }

        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/Order/EditOrderRemark").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/Order/CancelPendingOrder").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/Order/TerminateOrder").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/Order/BreachOrder").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/Order/RefundIn7Days").reply(() => {
        let ApiDataObject = { InvoiceNumber: "QC-55662340" }
        return [200, { ErrorCode: 1, ApiDataObject }]
    })
}