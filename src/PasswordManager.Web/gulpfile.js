var gulp = require('gulp');
var gutil = require('gulp-util');
var gulpif = require('gulp-if');
var autoprefixer = require('gulp-autoprefixer');
var cssmin = require('gulp-cssmin');
var less = require('gulp-less');
var concat = require('gulp-concat');
var plumber = require('gulp-plumber');
var buffer = require('vinyl-buffer');
var source = require('vinyl-source-stream');
var babelify = require('babelify');
var browserify = require('browserify');
var watchify = require('watchify');
var uglify = require('gulp-uglify');
var sourcemaps = require('gulp-sourcemaps');
var lint = require('gulp-eslint');

var dependencies = [
    'alt',
    'react',
    'react-dom',
    'react-router',
    'underscore'
];

gulp.task('vendor:production', function () {
    return gulp.src([
        'wwwroot/lib/jquery/dist/jquery.js',
        'wwwroot/lib/bootstrap/dist/js/bootstrap.js',
        'wwwroot/lib/magnific-popup/dist/jquery.magnific-popup.js',
        'wwwroot/lib/toastr/toastr.js'
    ]).pipe(concat('vendor.js'))
        .pipe(uglify({ mangle: false }))
        .pipe(gulp.dest('wwwroot/js'));
});


gulp.task('browserify-vendor:production', function () {
    return browserify()
        .require(dependencies)
        .bundle()
        .pipe(source('vendor.bundle.js'))
        .pipe(buffer())
        .pipe(uglify({ mangle: false }))
        .pipe(gulp.dest('wwwroot/js'));
});

gulp.task('browserify:production', ['browserify-vendor:production'], function () {
    return browserify({ entries: 'Client/app/main.js', debug: true })
        .external(dependencies)
        .transform(babelify, { presets: ['es2015', 'react'] })
        .bundle()
        .pipe(source('bundle.js'))
        .pipe(buffer())
        .pipe(sourcemaps.init({ loadMaps: true }))
        .pipe(uglify({ mangle: false }))
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest('wwwroot/js'));
});


//gulp.task('browserify-watch', ['browserify-vendor'], function () {
//    var bundler = watchify(browserify({ entries: 'app/main.js', debug: true }, watchify.args));
//    bundler.external(dependencies);
//    bundler.transform(babelify, { presets: ['es2015', 'react'] });
//    bundler.on('update', rebundle);
//    return rebundle();

//    function rebundle() {
//        var start = Date.now();
//        return bundler.bundle()
//            .on('error', function (err) {
//                gutil.log(gutil.colors.red(err.toString()));
//            })
//            .on('end', function () {
//                gutil.log(gutil.colors.green('Finished rebundling in', (Date.now() - start) + 'ms.'));
//            })
//            .pipe(source('bundle.js'))
//            .pipe(buffer())
//            .pipe(sourcemaps.init({ loadMaps: true }))
//            .pipe(sourcemaps.write('.'))
//            .pipe(gulp.dest('public/js/'));
//    }
//});


gulp.task('styles:production', function () {
    return gulp.src('Clinet/app/stylesheets/main.less')
        .pipe(plumber())
        .pipe(less())
        .pipe(autoprefixer())
        .pipe(cssmin())
        .pipe(gulp.dest('wwwroot/css'));
});

gulp.task('lint', function () {
    return gulp.src('bundle.js')
        .pipe(lint({ config: 'eslint.config.json' }))
        .pipe(lint.format());
});


gulp.task('watch', function () {
    gulp.watch('app/stylesheets/**/*.less', ['styles']);
});

gulp.task('default', ['styles', 'vendor', 'browserify-watch', 'lint', 'watch']);
gulp.task('build', ['styles', 'vendor', 'browserify']);