export default function (mock) {
    const orders = [
        {
            OrderId: 1,
            MemberId: 101,
            MemberName: "AA",
            MemberPhone: 987654321,
            PlanId: 201,
            PlanType: 2,
            PlanName: "健身月卡",
            OrderNumber: 202406170001,
            State: 1,
            Amount: 1200,
            Method: 2,
            Remark: "首次購買",
            InvoiceStatus: 3,
            CreateTime: "2025-06-17T10:01:00.123",
            UpdateTime: "2025-06-17T10:01:00.123"
        },
        {
            OrderId: 2,
            MemberId: 102,
            MemberName: "BB",
            MemberPhone: 987654322,
            PlanId: 202,
            PlanType: 2,
            PlanName: "瑜伽季卡",
            OrderNumber: 202406170002,
            State: 2,
            Amount: 3200,
            Method: 2,
            Remark: "升級方案",
            CreateTime: "2025-06-16T14:20:30.456",
            UpdateTime: "2025-06-16T14:20:30.456"
        },
        {
            OrderId: 3,
            MemberId: 103,
            MemberName: "CC",
            MemberPhone: 987654323,
            PlanId: 203,
            PlanType: 1,
            PlanName: "私人教練體驗",
            OrderNumber: 202406170003,
            State: 1,
            Amount: 600,
            Method: 1,
            Remark: "",
            CreateTime: "2025-06-15T09:10:05.789",
            UpdateTime: "2025-06-15T09:10:05.789"
        },
        {
            OrderId: 4,
            MemberId: 104,
            MemberName: "DD",
            MemberPhone: 987654324,
            PlanId: 204,
            PlanType: 3,
            PlanName: "團體課半年卡",
            OrderNumber: 202406170004,
            State: 3,
            Amount: 5400,
            Method: 2,
            Remark: "現場付款",
            CreateTime: "2025-06-14T12:00:00.000",
            UpdateTime: "2025-06-14T12:00:00.000"
        },
        {
            OrderId: 5,
            MemberId: 105,
            MemberName: "EE",
            MemberPhone: 987654325,
            PlanId: 205,
            PlanType: 1,
            PlanName: "健身單次票",
            OrderNumber: 202406170005,
            State: 1,
            Amount: 300,
            Method: 1,
            Remark: "贈送1次體驗",
            CreateTime: "2025-06-13T18:45:10.321",
            UpdateTime: "2025-06-13T18:45:10.321"
        },
        {
            OrderId: 6,
            MemberId: 106,
            MemberName: "FF",
            MemberPhone: 987654326,
            PlanId: 206,
            PlanType: 2,
            PlanName: "瑜伽月卡",
            OrderNumber: 202406170006,
            State: 2,
            Amount: 1500,
            Method: 2,
            Remark: "",
            CreateTime: "2025-06-12T11:15:20.999",
            UpdateTime: "2025-06-12T11:15:20.999"
        },
        {
            OrderId: 7,
            MemberId: 107,
            MemberName: "GG",
            MemberPhone: 987654327,
            PlanId: 207,
            PlanType: 1,
            PlanName: "舞蹈月卡",
            OrderNumber: 202406170007,
            State: 1,
            Amount: 1000,
            Method: 1,
            Remark: "推薦朋友加入",
            CreateTime: "2025-06-11T16:05:55.321",
            UpdateTime: "2025-06-11T16:05:55.321"
        },
        {
            OrderId: 8,
            MemberId: 108,
            MemberName: "HH",
            MemberPhone: 987654328,
            PlanId: 208,
            PlanType: 3,
            PlanName: "健身年卡",
            OrderNumber: 202406170008,
            State: 6,
            Amount: 9600,
            Method: 2,
            Remark: "信用卡付款",
            CreateTime: "2025-06-10T08:30:45.555",
            UpdateTime: "2025-06-10T08:30:45.555"
        },
        {
            OrderId: 9,
            MemberId: 109,
            MemberName: "II",
            MemberPhone: 987654329,
            PlanId: 209,
            PlanType: 2,
            PlanName: "私人教練三個月",
            OrderNumber: 202406170009,
            State: 1,
            Amount: 7500,
            Method: 1,
            Remark: "學生優惠",
            CreateTime: "2025-06-09T13:00:00.000",
            UpdateTime: "2025-06-09T13:00:00.000"
        }
    ];

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

    mock.onPost("/api/Order/GetOrderById").reply((config) => {
        let orderIdDto = JSON.parse(config.data);
        let orderTarget = orders.find(order => order.OrderId === Number(orderIdDto.OrderId));
        let orderStateTargets = orderStates.filter(orderState => orderState.OrderId === Number(orderIdDto.OrderId));

        const ApiDataObject = { Order: orderTarget, OrderStateList: orderStateTargets }
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