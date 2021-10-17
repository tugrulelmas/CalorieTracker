import { useState, useEffect } from 'react';
import DateFilter from '../../components/date-filter';
import { foodEntryService } from '../../services/food-entry-service';
import CalorieList from './calorie-list';
import { alertService } from '../../services/alert-service';
import moment from 'moment';

export default function CaloriePage() {
    const [calorieResult, setCalorieResult] = useState({});
    const [loading, setLoading] = useState(false);
    const [filter, setFilter] = useState({ page: 1, pageSize: 10, from: moment(new Date()).format('yyyy-MM-DD'), to: moment(new Date()).format('yyyy-MM-DD') });

    useEffect(() => {
        list(filter.page, filter.pageSize, filter.from, filter.to);
        return () => {
            setLoading(false);
            setCalorieResult({});
        }
    }, [filter]);

    const list = async (page, pageSize, from, to) => {
        setLoading(true);
        const data = await foodEntryService.getMyCalories(page, pageSize, from, to)
            .catch(e => {
                alertService.error(e.toString());
                return {};
            });
        setCalorieResult(data);
        setLoading(false);
    }

    const listFoodEntries = (dates) => {
        setFilter({ ...filter, from: dates.from, to: dates.to });
    }

    const onPageChange = (page) => {
        setFilter({ ...filter, page: page });
    }

    return (
        <div className="container px-6 mx-auto grid">
            <div
                className="flex my-6  items-center justify-between p-4 mb-8 text-sm font-semibold text-purple-100 bg-purple-600 rounded-lg shadow-md focus:outline-none focus:shadow-outline-purple">
                <div className="flex items-center">
                    <span className="text-xl"> Calories</span>
                </div>
            </div>

            <DateFilter onList={listFoodEntries} loading={loading} />

            <CalorieList calories={calorieResult.calories} pageSize={filter.pageSize} page={filter.page} totalCount={calorieResult.totalCount} onPageChange={onPageChange} />

            {!calorieResult || !calorieResult.calories || calorieResult.calories.length === 0 && <div className="px-4 py-3 mb-8 bg-white rounded-lg shadow-md">
                <span className="text-gray-700">No food entry found. </span>
            </div>}
        </div>
    )
}