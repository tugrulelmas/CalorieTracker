import { useState, useEffect } from 'react';
import DateFilter from './date-filter';
import { foodEntryService } from '../../services/food-entry-service';
import { alertService } from '../../services/alert-service';
import CalorieList from './calorie-list';

export default function ReportPage() {
    const [report, setReport] = useState({});
    const [loading, setLoading] = useState(false);
    const [filter, setFilter] = useState({ page: 1, pageSize: 10, to: new Date() });

    useEffect(() => {
        list(filter.page, filter.pageSize, filter.to);
        return () => {
            setReport({});
            setLoading(false);
        }
    }, [filter]);


    const list = async (page, pageSize, to) => {
        setLoading(true);
        const data = await foodEntryService.getReport(page, pageSize, to.toISOString())
            .catch(e => {
                alertService.error(e.toString());
                return {};
            });
        setReport(data);
        setLoading(false);
    }

    const onToChange = (to) => {
        setFilter({ ...filter, to: to });
    }

    const onPageChange = (page) => {
        setFilter({ ...filter, page: page });
    }

    return (
        <div className="container px-6 mx-auto grid">
            <div
                className="flex my-6  items-center justify-between p-4 mb-8 text-sm font-semibold text-purple-100 bg-purple-600 rounded-lg shadow-md focus:outline-none focus:shadow-outline-purple">
                <div className="flex items-center">
                    <span className="text-xl"> Report</span>
                </div>
            </div>
            <DateFilter onList={onToChange} loading={loading} />

            <div className="grid gap-6 mb-8 md:grid-cols-2 xl:grid-cols-4">
                <div className="flex items-center p-4 bg-white rounded-lg shadow-xs">
                    <div className="p-3 mr-4 text-orange-500 bg-orange-100 rounded-full">
                        <svg className="w-5 h-5" fill="currentColor" viewBox="0 0 20 20">
                            <path
                                d="M13 6a3 3 0 11-6 0 3 3 0 016 0zM18 8a2 2 0 11-4 0 2 2 0 014 0zM14 15a4 4 0 00-8 0v3h8v-3zM6 8a2 2 0 11-4 0 2 2 0 014 0zM16 18v-3a5.972 5.972 0 00-.75-2.906A3.005 3.005 0 0119 15v3h-3zM4.75 12.094A5.973 5.973 0 004 15v3H1v-3a3 3 0 013.75-2.906z"
                            ></path>
                        </svg>
                    </div>
                    <div>
                        <p className="mb-2 text-sm font-medium text-gray-600">
                            Total added entries
                        </p>
                        <p className="text-lg font-semibold text-gray-700">
                            {report.addedCount}
                        </p>
                    </div>
                </div>
            </div>
            <CalorieList calories={report.averageCalories} pageSize={filter.pageSize} page={filter.page} totalCount={report.totalCount} onPageChange={onPageChange} />
        </div>)
}