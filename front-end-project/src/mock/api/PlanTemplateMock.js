import avatar1 from '@/assets/mockImage/avatar1.png'
export default function (mock) {
    let membershipPlans = [
        {
            "MembershipPlanId": 1,
            "Name": "入門方案",
            "Price": 500,
            "Duration": 1,
            "Introduction": "適合初學者，提供基本健身設施使用權。",
            "ImageUrl": avatar1,
            "Display": true,
            "Status": true,
            "CreateTime": "2025-05-01T10:00:00.000",
            "UpdateTime": "2025-05-01T10:00:00.000"
        },
        {
            "MembershipPlanId": 2,
            "Name": "進階方案",
            "Price": 1200,
            "Duration": 3,
            "Introduction": "提供進階課程與更多設施使用。",
            "ImageUrl": avatar1,
            "Display": true,
            "Status": true,
            "CreateTime": "2025-05-01T10:05:00.000",
            "UpdateTime": "2025-05-01T10:05:00.000"
        },
        {
            "MembershipPlanId": 3,
            "Name": "半年計劃",
            "Price": 2200,
            "Duration": 6,
            "Introduction": "適合持續訓練者，享優惠價格與完整服務。",
            "ImageUrl": avatar1,
            "Display": true,
            "Status": true,
            "CreateTime": "2025-05-01T10:10:00.000",
            "UpdateTime": "2025-05-01T10:10:00.000"
        },
        {
            "MembershipPlanId": 4,
            "Name": "年度會員",
            "Price": 4000,
            "Duration": 12,
            "Introduction": "最優惠價格，含私人教練諮詢。",
            "ImageUrl": avatar1,
            "Display": true,
            "Status": true,
            "CreateTime": "2025-05-01T10:15:00.000",
            "UpdateTime": "2025-05-01T10:15:00.000"
        },
        {
            "MembershipPlanId": 5,
            "Name": "學生專案",
            "Price": 800,
            "Duration": 3,
            "Introduction": "限學生身份申請，需出示學生證。",
            "ImageUrl": avatar1,
            "Display": true,
            "Status": false,
            "CreateTime": "2025-05-01T10:20:00.000",
            "UpdateTime": "2025-05-01T10:20:00.000"
        }
    ]

    let personalTrainingPackages = [
        {
            "PersonalTrainingPackageId": 1,
            "Name": "體驗課程",
            "Price": 1500,
            "SessionCount": 3,
            "Introduction": "適合新手認識健身基礎動作。",
            "ImageUrl": "/images/pt1.jpg",
            "Display": true,
            "Status": true,
            "CreateTime": "2025-05-01T09:00:00.000",
            "UpdateTime": "2025-05-01T09:00:00.000"
        },
        {
            "PersonalTrainingPackageId": 2,
            "Name": "基礎訓練方案",
            "Price": 5000,
            "SessionCount": 10,
            "Introduction": "建立肌力與正確姿勢，專屬教練指導。",
            "ImageUrl": "/images/pt2.jpg",
            "Display": true,
            "Status": true,
            "CreateTime": "2025-05-01T09:30:00.000",
            "UpdateTime": "2025-05-01T09:30:00.000"
        },
        {
            "PersonalTrainingPackageId": 3,
            "Name": "進階塑型課程",
            "Price": 9000,
            "SessionCount": 20,
            "Introduction": "針對個人目標打造高強度訓練計畫。",
            "ImageUrl": "/images/pt3.jpg",
            "Display": true,
            "Status": true,
            "CreateTime": "2025-05-01T10:00:00.000",
            "UpdateTime": "2025-05-01T10:00:00.000"
        },
        {
            "PersonalTrainingPackageId": 4,
            "Name": "塑形挑戰班",
            "Price": 12000,
            "SessionCount": 25,
            "Introduction": "密集課程，達成短期身形轉變目標。",
            "ImageUrl": "/images/pt4.jpg",
            "Display": false,
            "Status": false,
            "CreateTime": "2025-05-01T10:30:00.000",
            "UpdateTime": "2025-05-01T10:30:00.000"
        }
    ]

    let ticketPlans = [
        {
            "TicketPlanId": 1,
            "Price": 100,
            "Status": true,
            "UpdateTime": "2025-05-01T08:00:00.000"
        },
        {
            "TicketPlanId": 2,
            "Price": 90,
            "Status": true,
            "UpdateTime": "2025-05-01T08:10:00.000"
        },
        {
            "TicketPlanId": 3,
            "Price": 80,
            "Status": false,
            "UpdateTime": "2025-05-01T08:15:00.000"
        }
    ]

    mock.onPost("/api/PlanTemplate/AddTicketPlan").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/PlanTemplate/AddMembershipPlan").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/PlanTemplate/AddPersonalTrainingPackage").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/PlanTemplate/GetMembershipPlan").reply((config) => {
        let {
            Status,
            SortOrder,
            SortOption,
            RecordPerPage,
            Page
        } = JSON.parse(config.data);

        Status = Status === "true" ? true : Status;
        Status = Status === "false" ? false : Status;

        // 1️⃣ 篩選
        let filtered = membershipPlans.filter(item => {
            const matchStatus = Status === null || item.Status === Status;
            return matchStatus;
        });

        let field;
        // 2️⃣ 排序
        if (SortOption === "price") {
            field = "Price";
        } else if (SortOption === "status") {
            field = "Status";
        } else {
            field = "MembershipPlanId"
        }

        filtered.sort((a, b) => {
            let aVal = a[field];
            let bVal = b[field];

            if (SortOption === 'status') {
                aVal = aVal ? 1 : 0;
                bVal = bVal ? 1 : 0;
            }

            if (aVal < bVal) return SortOrder === 'descending' ? 1 : -1;
            if (aVal > bVal) return SortOrder === 'descending' ? -1 : 1;
            return 0;
        });

        // 3️⃣ 分頁
        const start = (Page - 1) * RecordPerPage;
        const pageData = filtered.slice(start, start + RecordPerPage);

        const ApiDataObject = {
            MembershipPlanList: pageData,
            TotalPage: Math.ceil(
                filtered.length / RecordPerPage
            )
        }

        return [200, { ErrorCode: 1, ApiDataObject }]
    })

    mock.onPost("/api/PlanTemplate/GetPersionalTrainingPackage").reply((config) => {
        let {
            Status,
            SortOrder,
            SortOption,
            RecordPerPage,
            Page
        } = JSON.parse(config.data);

        Status = Status === "true" ? true : Status;
        Status = Status === "false" ? false : Status;

        // 1️⃣ 篩選
        let filtered = personalTrainingPackages.filter(item => {
            const matchStatus = Status === null || item.Status === Status;
            return matchStatus;
        });

        let field;
        // 2️⃣ 排序
        if (SortOption === "price") {
            field = "Price";
        } else if (SortOption === "status") {
            field = "Status";
        } else {
            field = "MembershipPlanId"
        }

        filtered.sort((a, b) => {
            let aVal = a[field];
            let bVal = b[field];

            if (SortOption === 'status') {
                aVal = aVal ? 1 : 0;
                bVal = bVal ? 1 : 0;
            }

            if (aVal < bVal) return SortOrder === 'descending' ? 1 : -1;
            if (aVal > bVal) return SortOrder === 'descending' ? -1 : 1;
            return 0;
        });

        // 3️⃣ 分頁
        const start = (Page - 1) * RecordPerPage;
        const pageData = filtered.slice(start, start + RecordPerPage);

        const ApiDataObject = {
            PersonalTrainingPackageList: pageData,
            TotalPage: Math.ceil(
                filtered.length / RecordPerPage
            )
        }

        return [200, { ErrorCode: 1, ApiDataObject }]
    })

    mock.onPost("/api/PlanTemplate/GetTicketPlan").reply((config) => {
        let {
            Status,
            SortOrder,
            SortOption,
            RecordPerPage,
            Page
        } = JSON.parse(config.data);

        Status = Status === "true" ? true : Status;
        Status = Status === "false" ? false : Status;

        // 1️⃣ 篩選
        let filtered = ticketPlans.filter(item => {
            const matchStatus = Status === null || item.Status === Status;
            return matchStatus;
        });

        let field;
        // 2️⃣ 排序
        if (SortOption === "price") {
            field = "Price";
        } else if (SortOption === "status") {
            field = "Status";
        } else {
            field = "MembershipPlanId"
        }

        filtered.sort((a, b) => {
            let aVal = a[field];
            let bVal = b[field];

            if (SortOption === 'status') {
                aVal = aVal ? 1 : 0;
                bVal = bVal ? 1 : 0;
            }

            if (aVal < bVal) return SortOrder === 'descending' ? 1 : -1;
            if (aVal > bVal) return SortOrder === 'descending' ? -1 : 1;
            return 0;
        });

        // 3️⃣ 分頁
        const start = (Page - 1) * RecordPerPage;
        const pageData = filtered.slice(start, start + RecordPerPage);

        const ApiDataObject = {
            TicketPlanList: pageData,
            TotalPage: Math.ceil(
                filtered.length / RecordPerPage
            )
        }

        return [200, { ErrorCode: 1, ApiDataObject }]
    })

    mock.onPost("/api/PlanTemplate/EditTicketPlanStatus").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/PlanTemplate/GetMembershipPlanEditDataById").reply((config) => {
        let getMembershipPlanByIdDto = JSON.parse(config.data);
        let membershipPlanTarget = membershipPlans.find(
            plan => plan.MembershipPlanId === Number(getMembershipPlanByIdDto.MembershipPlanId));

        if (membershipPlanTarget) {
            return [200, { ErrorCode: 1, ApiDataObject: membershipPlanTarget }]
        } else {
            return [200, { ErrorCode: 13 }]
        }
    })

    mock.onPost("/api/PlanTemplate/EditMembershipPlan").reply(() => {
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/PlanTemplate/GetPersonalTrainingPackageEditDataById").reply((config) => {
        let getPersonalTrainingPackageByIdDto = JSON.parse(config.data);
        let personalTrainingPackageTarget = personalTrainingPackages.find(
            plan => plan.PersonalTrainingPackageId === Number(getPersonalTrainingPackageByIdDto.PersonalTrainingPackageId));

        if (personalTrainingPackageTarget) {
            return [200, { ErrorCode: 1, ApiDataObject: personalTrainingPackageTarget }]
        } else {
            return [200, { ErrorCode: 13 }]
        }
    })

    mock.onPost("/api/PlanTemplate/EditPersonalTrainingPackage").reply(() => {
        return [200, { ErrorCode: 1 }]
    })
}